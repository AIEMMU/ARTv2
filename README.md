Automated Rodent and Whisker tracking software
================

A description of this software as well as detailed instructions and a tutorial
may be found <insert documentation link>

Issues
=========

[Issue Tracker](link to a git hub repositry)

Downloading
===========

Pre-built software is available for [download][].

[download]: link to the binary exe. files

See the instructions for building below.

Tip: Git on Windows
-------------------

1. Install [msysgit][].

2. You may need to run the following command so that the https protocol will work correctly

       git config --global http.sslcainfo "/c/Program Files (x86)/Git/bin/curl-ca-bundle.crt"

3. Clone the repository

       git clone <insert binary location>

[msysgit]: http://code.google.com/p/msysgit/

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