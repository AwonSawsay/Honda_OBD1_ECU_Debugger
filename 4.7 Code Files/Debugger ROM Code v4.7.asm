;##################################################################################
;#############    ECU Debugger ROM Code                               #############
;#############                                                        #############
;#############    Uncomment the setup section for the target ECU      #############
;#############    Uncomment the target patch section as well          #############
;#############    use asm662 to compile this file                     #############
;#############    Name the file to one of the following names         #############
;#############    and place in the debugger folder                    #############
;#############                                                        #############
;#############    P13DebuggerCode.code                                #############
;#############    P72DebuggerCode.code                                #############
;#############    P30DebuggerCode.code                                #############
;#############    HTSDebuggerCode.code                                #############
;#############    P13HTSDebuggerCode.code                             #############
;#############    CustomDebuggerCode.code                             #############
;#############                                                        #############
;##################################################################################

;------------------------- Data Stream Bytes ---------------------------------------



;Changelog:
;v1.0
;#########  Everything seems to be working. 
;Sends via serial in order r0 -r7 LRB ACC PC(Return Address) 0DEh 0ADh and finally a checksum byte (8bit total of all the preceeding bytes)
;Then the routine settles in an endless loop servicing the WDT
;When the loop is broken via the custom serial interupt the following will happen
;If byte "AA" was sent to the serial interupt, the routine will repeat the serial output and settle in the endless loop again
;If a 2 byte sequence is sent to the serial interupt it will replace the return address and the program will resume from that point

;*v3 Changes 
;	- Added X1, X2 and PSW to the items being sent. 
;	- Added a packet byte count bit after the end of packet bytes
;	- Changed the PC stack address to return from where it was called
;*V3.1 Changes
;	- Added a function to return to caller when INT sends BBBB
;	- Using RTI to restore PC, ACC, LRB, PSW 
;v3.2 Changes
;	- removed change pc and resend ability
;	- added ability to check if SCAl or cal was used to call routine
;	- switch X1 and X2 send bytes so they display "normal" on pc
;v3.3 changes
;	- removed and cleaned up code
;v3.4 changes
;	- Changed psw to use different datapointer.  The delay routine was overwriting it.
;	- Added further delays for serial TX so lower baud rates can be used.
;v3.5 changes
;	- removed serial interupt ability in favor of ostrich only "loop breaking"
;	- Fixed psw bug that caused x1 and x2 to not display properly
;	- changed PC return handling to use the byte following the call instruction to know how far back to set the return address
;	- Abandoned the SCAL vs CAl ID.  Only the format CAL will be used. eg "32 00 75 03" or (CAL 7500 followed by the return address offset)
;v3.6 changes
;	-Added DP register support.
;	- Removed serial packet "byte counting".  The extra byte after DP support was added was becoming problematic on the Windows side.
;	- Removed extra serial send delays and with it the suport for lower baud rates. 38400 works fine
;v3.7 changes
;	- Added SSP contents support
;v3.8 changes
;	- Re-arrange code for better efficiency
;v3.9 changes
;	- Change to LoopBreak byte. Now uses the CMPB value instead of a static byte eleswhere in the ROM
; 	- Bug fix from 3.8 involving psw
;	- Further code optimization
;v3.91
;	-Changed format to include the beginning code for the debugger.code files Now when changing code,
		;the reference addresses will be automatically updated so compiled code can be direct copied to 
		;.code file 
;v4.2 	- Added Asic feeder code to keep the MCU in control while debugging
;	  	- Added new trap loop code for better breakpoint control
;	  	- Added All current options for currently supported ECUs
;v4.3 	- Added more stack data to stream and stack depth awareness
;		- Added stack pointer to data stream
;       - Added byte count to the data stream 
;       - Stream is no longer static size due to varying stack contents
;		- Tweaked serial delay timing to speed up sending (Speeds lower than 38400 will not work)
;		- Added additional WDT and ASIC feed to accomodate longer sending times due to additional data in stream
;		- Code cleanup
;v4.4 	- Added additional ram output
;		- Changed endian'ness to be uniform 
;v4.5   - Added DP contents out
;v4.7	- Change Trap loops to require only 1 write "step" instead of 3

;Debugger Data Stream 
;
;Byte 0  - er0
;Byte 1  - er0
;Byte 2  - er1 
;Byte 3  - er1
;Byte 4  - er2 
;Byte 5  - er2
;Byte 6  - er3 
;Byte 7  - er3
;Byte 8  - [DP] rev
;Byte 9  - [DP] 
;Byte 10 - DP rev 
;Byte 11 - DP
;Byte 12 - X1 rev
;Byte 13 - X1 
;Byte 14 - X2 rev
;Byte 15 - X2
;Byte 16 - PSW rev
;Byte 17 - PSW
;Byte 18 - LRB rev
;Byte 19 - LRB
;Byte 20 - ACC rev
;Byte 21 - ACC
;Byte 22 - ModRET rev
;Byte 23 - ModRET 
;Byte 24 - ExtraRam1 
;Byte 25 - ExtraRam2
;Byte 26 - ExtraRam3 
;Byte 27 - ExtraRam4
;Byte 28 - ExtraRam5
;Byte 29 - ExtraRam6
;Byte 30 - SSP rev
;Byte 31 - SSP
;Byte 32 - Stack1 rev (Top of stack contents, Current RT address or PSW for ISR RTI))
;Byte 33 - Stack1 
;Byte 34 - Stack2 rev (LRB for ISR RTI)
;Byte 35 - Stack2 
;Byte 36 - Stack3 rev (ACC for ISR RTI)
;Byte 37 - Stack3
;Byte 38 - Stack4 rev (RT for ISR RTI) 
;Byte 39 - Stack4
;Byte 40 - Stack5 
;Byte 41 - Stack5
;Byte 42 - 0xDE 
;Byte 43 - 0xAD
;Byte 44 - ByteCount
;Byte 45 - Checksum
;Notes: rev = bytes are sent big endian

;#################### ECU Code Setup ##################
;Pay attention to the endian format for each setup item.  It's not consistent.


;;-------------- P12 p13 p14 Code Options --------------------------
DebuggerCodeAddress 	equ		07500h  ;(0HH,LL) Location to place the Debugger code in the bin file 
FirstBaudRateAddy		equ		00a06h	;(0HH,LL) Location of the first address to patch for baud rate
SecondBaudRateAddy		equ		00f30h	;(0HH,LL) Location of the second address to patch for baud rate 
BaudValue				equ		007h	;Byte to patch to baud rate addresses
LRBValue				equ		00076h	;(0HH,LLh) LRB value to use for debugger code
SerialDelay            equ     00080h  ;JRNZ Delay to allow serial byte to send 0-FF. 80h is good for 38400 baud
PSWValue				equ		00002h	;(0HH,LLh) PSW value to use for debugger code
IEValue 				equ		01000h	;(0HH,LLh) IE value to use for debugger code
STBUFFER 				equ		07ch	;STBUF Address 
ASICPORT				equ		P2IO	;Port address with asic feeder pin
AsicPin 				equ		002h				
StackBottom            equ     0047fh	;original Stack initialization address		
;Patch addresses need to be in the format 0HHLLh)
PatchAddress1			equ		06d56h	;Compatibility patch,allows p13 code on p12
Patch1					equ		002h    ;Compatibility patch,allows p13 code on p12
PatchAddress2			equ		06d57h  ;Compatibility patch,allows p13 code on p12
Patch2					equ		003h    ;Compatibility patch,allows p13 code on p12
PatchAddress3			equ		06d67h  ;Compatibility patch,allows p13 code on p12
Patch3					equ		012h    ;Compatibility patch,allows p13 code on p12
PatchAddress4			equ		06d68h  ;Compatibility patch,allows p13 code on p12
Patch4					equ		013h    ;Compatibility patch,allows p13 code on p12
PatchAddress5			equ		00d22h	;Checksum patch
Patch5					equ		003h    ;Checksum patch
PatchAddress6			equ		00d23h  ;Checksum patch
Patch6					equ		036h    ;Checksum patch
PatchAddress7			equ		00d24h  ;Checksum patch
Patch7					equ		00dh    ;Checksum patch
PatchAddress8			equ		00d25h  ;Checksum patch
Patch8					equ		000h    ;Checksum patch
PatchAddress9			equ		009dbh  ;Checksum patch
Patch9					equ		0cbh    ;Checksum patch
;;

;;-------------- HTS Code Options --------------------------
;DebuggerCodeAddress 	equ		07c2fh  ;(0HH,LL) Location to place the Debugger code in the bin file 
;FirstBaudRateAddy		equ		02612h	;(0HH,LL) Location of the first address to patch for baud rate
;SecondBaudRateAddy		equ		03402h	;(0HH,LL) Location of the second address to patch for baud rate 
;BaudValue				equ		0f8h	;Byte to patch to baud rate addresses
;SerialDelay            equ     00080h  ;JRNZ Delay to allow serial byte to send 0-FF. 80h is good for 38400 baud
;LRBValue				equ		0007eh	;(0HH,LLh) LRB value to use for debugger code
;PSWValue				equ		00002h	;(0HH,LLh) PSW value to use for debugger code
;IEValue 				equ		00002h	;(0HH,LLh) IE value to use for debugger code
;STBUFFER 				equ		051h	;STBUF Address 
;ASICPORT				equ		P2		;Port address with asic feeder pin
;AsicPin 				equ		010h	;ASIC feeder pin on 7u016 or whatever the chip is
;StackBottom            equ     0047eh	;original Stack initialization address			

;;-------------- P72 Code Options --------------------------
;DebuggerCodeAddress 	equ		07600h  ;(0HH,LL) Location to place the Debugger code in the bin file 
;FirstBaudRateAddy		equ		0270ch	;(0HH,LL) Location of the first address to patch for baud rate
;SecondBaudRateAddy		equ		037c3h	;(0HH,LL) Location of the second address to patch for baud rate 
;BaudValue				equ		0f8h	;Byte to patch to baud rate addresses
;SerialDelay            equ     00080h  ;JRNZ Delay to allow serial byte to send 0-FF. 80h is good for 38400 baud
;LRBValue				equ		0007eh	;(0HH,LLh) LRB value to use for debugger code
;PSWValue				equ		00002h	;(0HH,LLh) PSW value to use for debugger code
;IEValue 				equ		00002h	;(0HH,LLh) IE value to use for debugger code
;STBUFFER 				equ		051h	;STBUF Address 
;ASICPORT				equ		P2		;Port address with asic feeder pin
;AsicPin 				equ		010h					
;StackBottom            equ     0047fh	;original Stack initialization address		
;;
;;;Patch addresses need to be in the format (0LLHHh)
;PatchAddress1			equ		02a1dh	;Checksum Patch
;Patch1					equ		003h    ;Checksum Patch
;PatchAddress2			equ		02a1eh  ;Checksum Patch
;Patch2					equ		02ah    ;Checksum Patch
;PatchAddress3			equ		02a1fh  ;Checksum Patch
;Patch3					equ		02ah    ;Checksum Patch

;;-------------- P30 Code Options --------------------------
;DebuggerCodeAddress 	equ		06e8eh  ;(0HH,LL) Location to place the Debugger code in the bin file 
;FirstBaudRateAddy		equ		02565h	;(0HH,LL) Location of the first address to patch for baud rate
;SecondBaudRateAddy		equ		038d5h	;(0HH,LL) Location of the second address to patch for baud rate 
;BaudValue				equ		0f8h	;Byte to patch to baud rate addresses
;SerialDelay            equ     00080h  ;JRNZ Delay to allow serial byte to send 0-FF. 80h is good for 38400 baud
;LRBValue				equ		00021h	;(0HH,LLh) LRB value to use for debugger code
;PSWValue				equ		00002h	;(0HH,LLh) PSW value to use for debugger code
;IEValue 				equ		08000h	;(0HH,LLh) IE value to use for debugger code
;STBUFFER 				equ		051h	;STBUF Address 
;ASICPORT				equ		P2		;Port address with asic feeder pin
;AsicPin 				equ		010h
;StackBottom            equ     0047fh	;original Stack initialization address		
;PatchAddress1			equ		0285eh	;Checksum Patch
;Patch1					equ		0cbh    ;Checksum Patch
	
;		
;;-------------- P13 HTS Code Options --------------------------
;DebuggerCodeAddress 	equ		07d30h  ;(0HH,LL) Location to place the Debugger code in the bin file 
;FirstBaudRateAddy		equ		07fffh	;(0HH,LL) Location of the first address to patch for baud rate
;SecondBaudRateAddy		equ		07fffh	;(0HH,LL) Location of the second address to patch for baud rate 
;BaudValue				equ		0ffh	;Byte to patch to baud rate addresses
;SerialDelay            equ     00080h  ;JRNZ Delay to allow serial byte to send 0-FF. 80h is good for 38400 baud
;LRBValue				equ		00076h	;(0HH,LLh) LRB value to use for debugger code
;PSWValue				equ		00002h	;(0HH,LLh) PSW value to use for debugger code
;IEValue 				equ		01000h	;(0HH,LLh) IE value to use for debugger code
;STBUFFER 				equ		07ch	;STBUF Address 
;ASICPORT				equ		P2IO	;Port address with asic feeder pin
;AsicPin 				equ		002h					
;StackBottom            equ     0047fh	;original Stack initialization address		
;;-------------- Custom Code Options --------------------------
;DebuggerCodeAddress 	equ		07c2fh  ;(0HH,LL) Location to place the Debugger code in the bin file 
;FirstBaudRateAddy		equ		02612h	;(0HH,LL) Location of the first address to patch for baud rate
;SecondBaudRateAddy		equ		03402h	;(0HH,LL) Location of the second address to patch for baud rate 
;BaudValue				equ		0f8h	;Byte to patch to baud rate addresses
;SerialDelay             equ     00080h  ;JRNZ Delay to allow serial byte to send 0-FF. 80h is good for 38400 baud
;LRBValue				equ		0007eh	;(0HH,LLh) LRB value to use for debugger code
;PSWValue				equ		00002h	;(0HH,LLh) PSW value to use for debugger code
;IEValue 				equ		00002h	;(0HH,LLh) IE value to use for debugger code
;STBUFFER 				equ		051h	;STBUF Address 
;ASICPORT				equ		P2		;Port address with asic feeder pin
;AsicPin 				equ		010h	;ASIC feeder pin on 7u016 or whatever the chip is
;StackBottom             equ     0047eh	;original Stack initialization address		
;;--------------------------------------------------------------

;################## Code file header setup ##############

;This area should not be changed unless changes to the PC debugger code require it
;Do not change the order or remove anything without making a change to the PC app
 				org 0000h
;				DW DebuggerStart        ;Uncomment this if the Assembled binary will be dasm after building
;				org 0038h				;Uncomment this if the Assembled binary will be dasm after building
				DW PatchBytes
				DW DebuggerStart 		;Length of option bytes
				DW DebuggerCodeAddress
				DW FirstBaudRateAddy
				DW SecondBaudRateAddy
				DB BaudValue
				DW WhichLoop    		;WhichLoop Byte
				DW DebuggerLRB +1 		;Debugger LRB address
				DW DebuggerPSW +3 		;Debugger PSW address
				DW DebuggerIE +3  		;Debugger IE address
				DW Ram1 +1				;Extra Ram Address 1
				DW Ram2 +1				;Extra Ram Address 2
				DW Ram3 +1				;Extra Ram Address 3
				DW Ram4 +1				;Extra Ram Address 4
				DW Ram5 +1				;Extra Ram Address 5
				DW Ram6 +1				;Extra Ram Address 6				
PatchBytes:		
;########### Optional Patch code area #############

;Use this area to enable any ROM byte patches that are needed 
;Enter the addresses and patch value up top in the ECU specific code section
;				DW PatchAddress1			
;				DB Patch1					
;				DW PatchAddress2			
;				DB Patch2					
;				DW PatchAddress3			
;				DB Patch3					
;				DW PatchAddress4			
;				DB Patch4					
;				DW PatchAddress5			
;				DB Patch5					
;				DW PatchAddress6			
;				DB Patch6					
;				DW PatchAddress7			
;				DB Patch7					
;				DW PatchAddress8			
;				DB Patch8					
;				DW PatchAddress9			
;				DB Patch9					
				

DebuggerStart:	
;---------------------- Register Snapshot ----------------------------------
;Push to the stack a snapshot of all the registers the debugee code was using

				PUSHS 	A						;Push A to the stack
				PUSHS 	LRB						;Push LRB to stack							
				MOV 	A, PSW
				PUSHS 	A						;Push PSW 
				RB 		PSWH.0                  ;Disable interrupts
				L 		A, X2					
				PUSHS 	A						;-Push X2 to stack
				L 		A, X1 					;
				PUSHS 	A						;-Push X1 to stack
				MOV 	A, DP
				PUSHS 	A						;Push DP to the stack
				L   	A, er3
				PUSHS	A						;-Push er3 to the stack
				L   	A, er2
				PUSHS	A						;-Push er2 to the stack
				L   	A, er1
				PUSHS	A						;-Push er1 to the stack
				L    	A, er0

;---------------- Send Snapshot and store registers for RTI ----------------
;Change LRB and PSW so the registers can be used without affecting the code being debugged
;Then Pop all the preserved registers and send via serial

DebuggerLRB:	MOV     LRB, #LRBValue
DebuggerPSW:	MOV		PSW, #PSWValue
				CLR 	er3                     ;Clear er3 so r6 and r7 can be used for byte count and checksum
				SCAL 	SendWord
				MOVB 	r3, #001h
				SCAL 	Feeder2Extension        ;Service WDT and Asic 				
				POPS 	A 						;Pop er1  
				SCAL 	SendWord		
				POPS 	A 						;Pop er2  	
				SCAL 	SendWord		
				POPS 	A 						;Pop er3  	
				SCAL 	SendWord		
				POPS 	A 						;Pop DP  		
				PUSHS 	A                       ;Push DP back
				AND 	A, #0FFFEh				;Respect word boundary
				MOV 	DP, A                   
				L       A, [DP]                 
				SCAL 	SwapThenSend			;Send DP Contents
				POPS 	A						;Pop DP
				SCAL 	SwapThenSend
				POPS 	A 						;Pop X1  		
				SCAL 	SwapThenSend		
				POPS 	A 						;Pop X2  
				SCAL 	SwapThenSend		
				POPS 	A 						;Pop PSW  	
				ST 		A, er2					;Store PSW in er0
				SCAL 	SwapThenSend		
				POPS 	A 						;Pop LRB 
				ST 		A, er1					;Store LRB in er1
				SCAL    SwapThenSend		
				POPS 	A 						;Pop ACC		  
				ST		A, er0					;Store ACC in er0
				SCAL 	SwapThenSend			;Send ACC

;---------------- Return Address Adjustment --------------------------------           					
;This part will check the byte trailing the debugger call instruction
;and use it to offset the return address 

AdjustReturn:   POPS    A               		;Pop the return address
				PUSHS 	A						;Push return address back
                LCB		A, [ACC]				;Load the return address modifier into A
												;When the debugger code was called, an offset bit was placed in the location where a 
												;RT would normally resume the code. When CAl is executed this address is pushed to the stack
												;The offset value is use to determine what the return address should be when leaving the debugger code
				MOV 	DP, A		
				POPS 	A		
pc_modifier:	SUB 	A, #00001h				;Subtract from the return address per the modifier
				JRNZ 	DP, pc_modifier 		
						
				PUSHS   A               		;Push the modified return address back where we got the original
				SCAL    SwapThenSend    		;Send the return address
                SJ 		GetRamValues	
;--------------------Jump extension ----------------------------------------
Feeder2Extension:SJ		WDT_feeder2
;---------------------------------------------------------------------------
;----------------------------- Serial --------------------------------------
;(Moved here so SCAL can reach)

SwapThenSend:	SWAP
SendWord:		SCAL  	UpdChk_SendA
				L 		A, ACC
				SWAP
				
UpdChk_SendA:   LB      A, ACC                  
                ADDB 	r7, A                   
				STB 	A, STBUFFER				;Send Byte via serial
				INCB 	r6						;Byte count ++
				MOV     DP, #SerialDelay         ;Serial delay amount            
Delay_me:       JRNZ    DP, Delay_me            ;Loop delay to give the serial buffer time to send          
				RT                          
;----------------- Send Checksum and Byte count ----------------------------
send_chksum:	L 		A, #0DEADh				;End of packet bytes
				SCAL 	SwapThenSend 
				incb	r6                      ;Byte count +2 for checksum and bytecount bytes
				incb    r6
				LB 		A, r6
				SCAL 	UpdChk_SendA
				LB 		A, r7					;Checksum to A
				STB 	A, STBUFFER					;Send Checksum
				RT

;--------------- Selective RAM Contents ------------------------------------
GetRamValues: 	
Ram1:			MOV  	DP, #00220h
				LB		A, [DP]
				SCAL 	UpdChk_SendA
Ram2:			MOV  	DP, #00222h
				LB		A, [DP]
				SCAL 	UpdChk_SendA
Ram3:			MOV  	DP, #00220h
				LB		A, [DP]
				SCAL 	UpdChk_SendA
Ram4:			MOV  	DP, #00222h
				LB		A, [DP]
				SCAL 	UpdChk_SendA
Ram5:			MOV  	DP, #00222h
				LB		A, [DP]
				SCAL 	UpdChk_SendA
Ram6:			MOV  	DP, #00222h
				LB		A, [DP]
				SCAL 	UpdChk_SendA				
				SJ 		SendSSPandStack


;--------------- WDT and Asic feeder ---------------------------------------
;These feeders keep the MCU from resetting and the 7U016 Chip from taking over the ECU
WDT_feeder: 	MOVB 	r3, #013h
WDT_feeder2:	XORB    ASICPORT, #AsicPin	 ;Toggles Asic feeder
				LB      A, #03ch             ;Watchdog feeder
                STB     A, WDT               
                SWAPB                          
                STB     A, WDT  

;10.19ms delay to set the Asic feeder and then RT 
delayfeeder:	DECB 	r3
				CMPB 	r3, #000h
				JEQ 	ExitFeeder				
				MOV		DP, #0ffffh
Delay2:		    JRNZ 	DP, Delay2
				SJ 		delayfeeder
ExitFeeder:	 	RT
;-------------- Send SSP and Stack Contents --------------------------------	
SendSSPandStack:
				MOV 	X2, SSP					;Preserve the stack pointer in X2
				MOV 	X1, X2
				MOV 	A, X2
				ADD 	A, #00002h				;Adjust the SSP being sent to reflect the ssp the debugee code sees
				SCAl 	SwapThenSend            ;Send the Stack pointer
SendStack:		INC 	X1                      
				INC 	X1                      ;Loop through sending stack contents using X1 as an increasing SSP 
				CMP 	X1, #StackBottom  - 002h ;Stop sending when stack bottom is reached or 5 items have sent
				JGT 	DoneSending
				MOV 	SSP, X1
                POPS 	A 						;Pop and send stack contents, restoring SSP before calling the send 
				MOV 	SSP, X2
                SCAL 	SwapThenSend
				MOV 	A, X1
				SUB 	A, X2
				CMP 	A, #0000ah				;Maximum contents to send (x2 for 2 bytes each)
				JGE 	DoneSending
				SJ 		SendStack

DoneSending:    SCAL   	send_chksum

;--------------- Prepare the stack for RTI ---------------------------------
				L 		A, er0					;Retrieve ACC then push to stack
				Pushs 	A					
				L 		A, er1					;load LRB and push to stack
				PUSHS 	A
				L		A, er2					;Retrieve PSW then push to stack
				PUSHS 	A					
DebuggerIE:		MOV 	IE, #IEValue			;Enable only Serial Interrupts while in the waiting loop
				SB 		PSWH.0					;Enable interrupts
				
;-------------------- Trap Waiting Loops -----------------------------------
;The alternating use of these two loops allows the emulator time to break and restore the loops
;without the ECU looping back through the code before the write is complete	
;The PC app changes the WhichLoop byte to control which loop is used.	
EnterTraps:     SCAL    GetDBAddressInX1
				LCB 	A, [X1]
				JNE     TrapLoop2
				
TrapLoop1:		SCAL 	WDT_feeder
				LCB 	A, [X1]
				JEQ 	TrapLoop1
				RTI
				
TrapLoop2:		SCAL 	WDT_feeder
				LCB 	A, [X1]
				JNE 	TrapLoop2
				RTI

;This following code is necessary to keep the debugger code dynamic as it's moved around different ROMS
;Since we don't know where it will end up after compiling , the following code finds out where it is and creates a pointer to the 
;WhichLoop byte that the PC app will change to break/make the trap loops

GetDBAddressInX1:   SCAL 	PopRomAddress		;This pushes the ROM address that follows the call to the stack
PopRomAddress:  POPS	A						;This retrieves the ROM address
				ADD 	A, #(WhichLoop - PopRomAddress) ;When we add this distance, we essentially get a pointer to WhichLoop byte
				MOV 	X1, A
				RT 	    	
WhichLoop:      DB      000h

;---------------------------------------------------------------------------


