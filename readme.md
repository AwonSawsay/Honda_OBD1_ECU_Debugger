
### ECU Debugger  
This program will allow ECU "Debugging" on Honda's OBD 1 computers.   
Step through ECU code code line by line and monitor the exact changes  
as the code is executed.  Great for debugging new features or getting a   
better understanding of code flow.  

Built in support for ECUS running P12, P13, P14, P28, P72, P30, ecTune, HTS codebases.  
Additional ECU support can be added by the user via included .code files and documentation.  
  
The following registers are monitored and logged  
-ACC  
-r0 to r7  
-PSW (CF, ZF, HC, DD, SF, MIE, SCB, BCB)  
-X1  
-X2  
-DP  
-[DP]  
-LRB  
-SSP  
-5 Layers of stack contents  
-Current Instruction RAM  
-User selectable RAM Watch  
  
New Features:  
-Load source from an ASM file to automatically compile and upload to emulator upon changes  
-Load source from an BIN file to automatically decompile and upload to emulator upon changes  
-Force/Ignore missed or false addresses in ASM files  
-VCAL Re-routing option - Now allows 100% breakpoint instruction compatibility  
  
  
Requirements:  
	-Moates Ostrich (Tested with v2)  
	-Serial datalogging device connected to ECU CN2 header and PC  
	-Engine SIM to power ECU (Vehicle connection is NOT recommended)  
  	  
![screenshot](https://user-images.githubusercontent.com/86896465/209742203-580f1bbd-2916-4f9c-89fc-3db92d30aff8.jpg)



