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

### Disclaimer
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
