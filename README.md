###OQAT - Objective Quality Assessment Toolkit

####About

OQAT was developed and build in 2012 as a project for the "Praxis der Softwareentwicklung" 
course at the [KIT](http://kit.edu). The main objective of OQAT is to provide means for objective
quality assessment of video encoding (or any kind of video processing) software. An other important 
thing about OQAT is its modularity, you can extend nearly every part of this software with plugins. 

####How does it work?
The process of using OQAT is simple enough, lets say you are developing a video encoder and it
produces poor results if certain distortions in the source video occur. At this point already
you can use OQAT. OQAT in its current state can only read/write/display YUV420 videos, but you can write 
a __Video Handler Plugin__ for any format you like and just drop it into your plugin folder, OQAT will then
be able to use the new format with all your other plugins. As it turns out you dont have most of your 
reference video files with that kind of distortion. So you write (or use one of the provided, 
if it fits your needs) a __Filter Plugin__ (takes one video, distortes it in some way and writes 
the result to disk) for OQAT and execute it on all your videos. After that you can begin to develop 
a fix for your encoder. To be able to document objectively about the effectiveness of your fix you 
can write a __Metric Plugin__ (some are provided) and execute it (two input videos, one output video 
according to the implemented quality metric) on the results the encoder produced before the fix and 
he videos it produces with your fix. To be able to get the most information out of your __Metric Plugin__ 
you can write a __Presentation Plugin__ to format the data in some meaningfull way. 

On top of the functionality described above there are many more feateures, like 
  * Project Handling
  you can organize your diffierent projects and analyses into projects.
  * Task Queue 
  This is provided by the only inextendible plugin, called __Macro Plugin__. The Task Queue
  lets you arrange your filters/metrics/videos in a certain order and duration of execution (e.g. 
  execute _Noise Filter_ only the first 10 seconds but after your Custom Filter was executed).
  
  * Extensive use of Plugins
  Nearly every element of OQAT (except some GUI elements) is a plugin and can be extended, which leaves the
  user very much control over how OQAT works.

#####Technology

  * [.NET 4.0 Framework](http://msdn.microsoft.com/en-us/vstudio/aa496123.aspx)
  * [Windows Presentation Foundation](http://msdn.microsoft.com/en-us/library/aa970268.aspx)
  * Dependency Injection using [MEF](http://msdn.microsoft.com/en-us/library/dd460648.aspx)
  * Unit testing using [Visual Studio Unit Testing Framework](http://msdn.microsoft.com/en-us/library/ms243147.aspx)
  * [Coded UI Tests with Visual Studio](http://msdn.microsoft.com/en-us/library/dd286726.aspx)
  * [Avalon Controls Library](http://avaloncontrolslib.codeplex.com/)
  * [Extended WPF Toolkit](http://wpftoolkit.codeplex.com/)
  * [AForge.NET Framework](http://www.aforgenet.com/)


#####Note
Even if you read the above carefully you still wouldn't know half of OQATs functionallity, you should either try it out
(a compiled version can be found in /OQAT_release) or dive into the code (in /Implementierung). If you understand German 
you could have a look into /docs/Entwurf.pdf for some general overview on OQATs architecture.

 

