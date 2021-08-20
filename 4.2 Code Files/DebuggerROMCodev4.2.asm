;##################################################################################
;#############    ECU Debugger ROM Code                               #############
;#############                                                        #############
;#############    Uncomment the setup section for the target ECU      #############
;#############    Uncomment the target patch section as well          #############
;#############    use asm662 to compile this file                     #############
;#############    Use a hex editor to remove all FF bytes after code  #############
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
;v4.2 - Added Asic feeder code to keep the MCU in control while debugging
;	  - Added new trap loop code for better breakpoint control
;	  - Added All current options for currently supported ECUs


 


;#################### ECU Code Setup ##################
;Pay attention to the endian format for each setup item.  It's not consistent.


;;-------------- P12 p13 p14 Code Options --------------------------
;DebuggerCodeAddress 	equ		00075h  ;(0LL,HHh) Location to place the Debugger code in the bin file 
;FirstBaudRateAddy		equ		0060ah	;(0LL,HHh) Location of the first address to patch for baud rate
;SecondBaudRateAddy		equ		0300fh	;(0LL,HHh) Location of the second address to patch for baud rate 
;BaudValue				equ		070h	;Byte to patch to baud rate addresses
;LRBValue				equ		00076h	;(0HH,LLh) LRB value to use for debugger code
;PSWValue				equ		00002h	;(0HH,LLh) PSW value to use for debugger code
;IEValue 				equ		01000h	;(0HH,LLh) IE value to use for debugger code
;STBUFFER 				equ		07ch	;STBUF Address 
;ASICPORT				equ		P2IO	;Port address with asic feeder pin
;AsicPin 				equ		002h				
;;Patch addresses need to be in the format 0LLHHh)
;PatchAddress1			equ		0566dh	;Compatibility patch,allows p13 code on p12
;Patch1					equ		002h    ;Compatibility patch,allows p13 code on p12
;PatchAddress2			equ		0576dh  ;Compatibility patch,allows p13 code on p12
;Patch2					equ		003h    ;Compatibility patch,allows p13 code on p12
;PatchAddress3			equ		0676dh  ;Compatibility patch,allows p13 code on p12
;Patch3					equ		012h    ;Compatibility patch,allows p13 code on p12
;PatchAddress4			equ		0686dh  ;Compatibility patch,allows p13 code on p12
;Patch4					equ		013h    ;Compatibility patch,allows p13 code on p12
;PatchAddress5			equ		0220dh	;Checksum patch
;Patch5					equ		003h    ;Checksum patch
;PatchAddress6			equ		0230dh  ;Checksum patch
;Patch6					equ		036h    ;Checksum patch
;PatchAddress7			equ		0240dh  ;Checksum patch
;Patch7					equ		00dh    ;Checksum patch
;PatchAddress8			equ		0250dh  ;Checksum patch
;Patch8					equ		000h    ;Checksum patch
;PatchAddress9			equ		0db09h  ;Checksum patch
;Patch9					equ		0cbh    ;Checksum patch
;;

;;-------------- HTS Code Options --------------------------
DebuggerCodeAddress 	equ		02f7ch  ;(0LL,HHh) Location to place the Debugger code in the bin file 
FirstBaudRateAddy		equ		01226h	;(0LL,HHh) Location of the first address to patch for baud rate
SecondBaudRateAddy		equ		00234h	;(0LL,HHh) Location of the second address to patch for baud rate 
BaudValue				equ		070h	;Byte to patch to baud rate addresses
LRBValue				equ		0007eh	;(0HH,LLh) LRB value to use for debugger code
PSWValue				equ		00002h	;(0HH,LLh) PSW value to use for debugger code
IEValue 				equ		00002h	;(0HH,LLh) IE value to use for debugger code
STBUFFER 				equ		051h	;STBUF Address 
ASICPORT				equ		P2		;Port address with asic feeder pin
AsicPin 				equ		010h				
;
;;-------------- P72 Code Options --------------------------
;DebuggerCodeAddress 	equ		00076h  ;(0LL,HHh) Location to place the Debugger code in the bin file 
;FirstBaudRateAddy		equ		00c27h	;(0LL,HHh) Location of the first address to patch for baud rate
;SecondBaudRateAddy		equ		0c337h	;(0LL,HHh) Location of the second address to patch for baud rate 
;BaudValue				equ		070h	;Byte to patch to baud rate addresses
;LRBValue				equ		0007eh	;(0HH,LLh) LRB value to use for debugger code
;PSWValue				equ		00002h	;(0HH,LLh) PSW value to use for debugger code
;IEValue 				equ		00002h	;(0HH,LLh) IE value to use for debugger code
;STBUFFER 				equ		051h	;STBUF Address 
;ASICPORT				equ		P2		;Port address with asic feeder pin
;AsicPin 				equ		010h					
;
;;Patch addresses need to be in the format (0LLHHh)
;PatchAddress1			equ		01d2ah	;Checksum Patch
;Patch1					equ		003h    ;Checksum Patch
;PatchAddress2			equ		01e2ah  ;Checksum Patch
;Patch2					equ		02ah    ;Checksum Patch
;PatchAddress3			equ		01f2ah  ;Checksum Patch
;Patch3					equ		02ah    ;Checksum Patch

;;-------------- P30 Code Options --------------------------
;DebuggerCodeAddress 	equ		08e6eh  ;(0LL,HHh) Location to place the Debugger code in the bin file 
;FirstBaudRateAddy		equ		06525h	;(0LL,HHh) Location of the first address to patch for baud rate
;SecondBaudRateAddy		equ		0d538h	;(0LL,HHh) Location of the second address to patch for baud rate 
;BaudValue				equ		070h	;Byte to patch to baud rate addresses
;LRBValue				equ		00021h	;(0HH,LLh) LRB value to use for debugger code
;PSWValue				equ		00002h	;(0HH,LLh) PSW value to use for debugger code
;IEValue 				equ		08000h	;(0HH,LLh) IE value to use for debugger code
;STBUFFER 				equ		051h	;STBUF Address 
;ASICPORT				equ		P2		;Port address with asic feeder pin
;AsicPin 				equ		010h
;
;;Patch addresses need to be in the format (0LLHHh)
;PatchAddress1			equ		05e28h	;Checksum Patch
;Patch1					equ		0cbh    ;Checksum Patch
	
;		
;;-------------- P13 HTS Code Options --------------------------
;DebuggerCodeAddress 	equ		0307dh  ;(0LL,HHh) Location to place the Debugger code in the bin file 
;FirstBaudRateAddy		equ		0ff7fh	;(0LL,HHh) Location of the first address to patch for baud rate
;SecondBaudRateAddy		equ		0ff7fh	;(0LL,HHh) Location of the second address to patch for baud rate 
;BaudValue				equ		0ffh	;Byte to patch to baud rate addresses
;LRBValue				equ		00076h	;(0HH,LLh) LRB value to use for debugger code
;PSWValue				equ		00002h	;(0HH,LLh) PSW value to use for debugger code
;IEValue 				equ		01000h	;(0HH,LLh) IE value to use for debugger code
;STBUFFER 				equ		07ch	;STBUF Address 
;ASICPORT				equ		P2IO	;Port address with asic feeder pin
;AsicPin 				equ		002h					

;;-------------- Custom Code Options --------------------------
;DebuggerCodeAddress 	equ		0307dh  ;(0LL,HHh) Location to place the Debugger code in the bin file 
;FirstBaudRateAddy		equ		0ff7fh	;(0LL,HHh) Location of the first address to patch for baud rate
;SecondBaudRateAddy		equ		0ff7fh	;(0LL,HHh) Location of the second address to patch for baud rate 
;BaudValue				equ		0ffh	;Byte to patch to baud rate addresses
;LRBValue				equ		00076h	;(0HH,LLh) LRB value to use for debugger code
;PSWValue				equ		00002h	;(0HH,LLh) PSW value to use for debugger code
;IEValue 				equ		01000h	;(0HH,LLh) IE value to use for debugger code
;STBUFFER 				equ		07ch	;STBUF Address 
;ASICPORT				equ		P2IO	;Port address with asic feeder pin
;AsicPin 				equ		002h				
;;--------------------------------------------------------------

;################## Code file header setup ##############

;This area should not be changed unless changes to the debugger code require it
 				
				org 0000h
				DB DebuggerStart 		;Length of option bytes
				DW DebuggerCodeAddress
				DW FirstBaudRateAddy
				DW SecondBaudRateAddy
				DB BaudValue
				DB WhichLoop +1 		;WhichLoop Byte
				DB TrapLoop1 +1		 	;TrapLoop1 breakbyte
				DB TrapLoop2 +1			;TrapLoop2 breakbyte
				DB DebuggerLRB +1 		;Debugger LRB address
				DB DebuggerPSW +3 		;Debugger PSW address
				DB DebuggerIE +3  		;Debugger IE address
		
;########### Optional Patch code area #############

;Use this area to enable any patches that are specified in the code
;section for the ECU that you are debugging.

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
;Push all the registers we want to send up to the stack

				PUSHS 	A						;Push A to the stack
				PUSHS 	LRB						;Push LRB to stack							
				MOV 	A, PSW
				PUSHS 	A						;Push PSW 
				RB 		PSWH.0
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

;Change LRB so we can use the er registers and change SCB so we can use the DP
;Then Pop all the registers and send via serial


DebuggerLRB:	MOV     LRB, #LRBValue
DebuggerPSW:	MOV		PSW, #PSWValue
				SCAL 	send_word
				POPS 	A 					   ;Pop er1  
				SCAL 	send_word
				POPS 	A 					   ;Pop er2  	
				SCAL 	send_word
				POPS 	A 					   ;Pop er3  	
				SCAL 	send_word
				POPS 	A 					   ;Pop DP  		
				SCAL 	SwapThenSend
				POPS 	A 					   ;Pop X1  		
				SCAL 	SwapThenSend
				POPS 	A 					   ;Pop X2  
				SCAL 	SwapThenSend
				POPS 	A 					   ;Pop PSW  	
				ST 		A, er2				   ;Store PSW in er0
				SCAL 	SwapThenSend
				POPS 	A 					   ;Pop LRB 
				ST 		A, er1				   ;Store LRB in er1
				SCAL    SwapThenSend
				POPS 	A 					   ;Pop ACC		  
				ST		A, er0				   ;Store ACC in er0
				SCAL 	SwapThenSend		   ;Send ACC

;This part will check the byte trailing the call instruction, and use it to offset the return address 

			    POPS    A                      ;Pop the return address
				PUSHS 	A					   ;Push return address back
                LCB		A, [ACC]			   ;Load the return address modifier into A
				MOV 	DP, A
				POPS 	A
pc_modifier:	SUB 	A, #00001h			   ;Subtract from the return address per the modifier
				JRNZ 	DP, pc_modifier 	
				
				PUSHS   A                      ;Push the return address back where we got it
				SCAL    SwapThenSend           ;Send the return address
                CLR 	A
				ADD 	SSP, #00002h
				POPS 	A					   ;This was the bottom of the stack before the debugger was called
				SUB 	SSP, #00004h
				SCAL    SwapThenSend           ;Send the address in SSP
				scal   	send_chksum
				L 		A, er0				;Retrieve ACC then push to stack
				Pushs 	A					
				L 		A, er1				;load LRB and push to stack
				PUSHS 	A
				L		A, er2				;Retrieve PSW then push to stack
				PUSHS 	A					
DebuggerIE:		MOV 	IE, #IEValue		;Enable only Serial Interrupt
				SB 		PSWH.0				;Enable interrupts
				
;------------Trap Loops---------------------
;The alternating use of these two loops allows the emulator time to break and restore the loops
;without the ECU looping back through the code before the write is complete	
;The PC app changes the bytes that are MOVB into r0,r1, and r2 to break, restore, and select the loops		
WhichLoop: 		MOVB 	r0, #000h           
				LB		A, r0
				JNE 	TrapLoop2
				
TrapLoop1:		MOVB	r1, #000h           ;Second loop break byte -
				SCAL 	WDT_feeder
				CMPB 	r1, #000h
				JEQ 	TrapLoop1
				RTI
				
TrapLoop2:		MOVB 	r2, #000h
				SCAL 	WDT_feeder
				CMPB 	r2, #000h
				JNE 	TrapLoop2
				RTI
;------------------------------------------
send_chksum:	L 		A, #0DEADh				;End of packet bytes
				scal 	SwapThenSend 
				LB 		A, r7					;Checksum to A
				Stb 	A, STBUFFER					;Send Checksum
				clr		er3
				RT
;------------------Serial--------------------

SwapThenSend:	SWAP
send_word:		SCAL  	ChkSum_n_send
				L 		A, ACC
				SWAP
				
ChkSum_n_send:  LB      A, ACC                  
				PUSHS 	A						;Push the byte to send to the stack 
				ADDB    A, r7					;Add checksum to byte to send
				STB     A, r7					;store updated checksum	
				POPS 	A						;Get back the byte we're sending
                MOVB 	STBUFFER, A					;Send Byte via serial
				MOV     DP, #000ffh             ;            
Delay_me:       JRNZ    DP, Delay_me            ;-Loop delay to give the serial buffer time to send          
		
				RT                             
;---------------WDT and Asic feeder-----------------------
WDT_feeder: 	CLRB 	r3
				XORB    ASICPORT, #AsicPin	 ;Toggles Asic feeder
				LB      A, #03ch             ;Watchdog feeder
                STB     A, WDT               
                SWAPB                          
                STB     A, WDT  

;10.19ms delay to set the Asic feeder and then RT 
delayfeeder:	MOV		DP, #0ffffh
testdelay:		JRNZ 	DP, testdelay
				INCB 	r3
				CMPB 	r3, #012h
				JNE 	delayfeeder				
				
				RT
;-------------------------------------             	
				
			

				



				
				
				
				
