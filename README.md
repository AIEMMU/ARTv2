Automated Rodent and Whisker tracking software
================

A description of this software as well as detailed instructions and a tutorial
may be found <insert documentation link>

Building
========

This software requires visual studio to build and run, but it is possible to port this to MacOS or linux. It requires a custom Emgu package that is provided in this repositry.
To install on windows. 

On Windows and OS X, you'll have to install developer tools.  On Windows,
   that's [Visual Studio](http://www.microsoft.com/express); one of the free
   "Express Edition" versions is fine.  O

5. Start in the root of the source directory (in a terminal).
   The contents of this directory look something like this:

      	3.1 Custom DLLS/
	ArtLibrary/
	ARWT/
	ARWT-Master/
	AutomatedRodentTracker/

The visual studio .sln can be found in ARWT-Master/AutomatedRodentWhiskerTracker

By clicking on the .sln, the project should be opened up. This will allow you to customise
the code within the source files, and build and run the .exe from scratch. 


File formats
============

### Rodent and Whisker detection

The sotware loads in .avi video files and processes these to detect rodents and 
whiskers within the video file. 

The software exports data calculated from the .avi files as an .xlsl file and a .ARWT
file which stores the variables and such from the video, allowing you to directly the 
data back into the software. 

### Citations and Issues

if you use this code, please cite
```text
@article{GILLESPIE2019108440,
title = "Description and validation of the LocoWhisk system: Quantifying rodent exploratory, sensory and motor behaviours",
author = "David Gillespie and Moi Hoon Yap and Brett M. Hewitt and Heather Driscoll and Ugne Simanaviciute and Emma F. Hodson-Tole and Robyn A. Grant",
journal = "Journal of Neuroscience Methods",
doi = "https://doi.org/10.1016/j.jneumeth.2019.108440",
url = "http://www.sciencedirect.com/science/article/pii/S0165027019302973",
}
```

If you have any issues or questions, please direct them either to this repository or email d.gillespie@mmu.ac.uk 