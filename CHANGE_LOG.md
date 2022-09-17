# EVA Follower /L Unleashed :: Change Log

* 2022-0917: Release 1.2.0.1 (LisiasT) for KSP >= 1.3.1
	+ Updates to the last KSPe version (2.4.2.x).
	+ Fixes some small code mishaps
	+ Adds support for new EVA Suits on 1.12
* 2021-1109: Release 1.2.0.0 (LisiasT) for KSP >= 1.3.1
	+ Using KSPe facilities (Logging, abstracted File System, etc)
	+ Updating it to run also on moderns KSPs (from 1.3.1 to the latest! **#HURRAY!!**)
		- Command Seats
		- Kerbal Suits from Making History and Serenity
		- Shaders on KSP >= 1.8
	+ Small performance enhancements 
* 2016-0520: Release 1.0.35 (MSD) for KSP 1.1.2
	+ Compatability for release 1.1.2
* 2016-0424: Release 1.0.33 (MSD) for KSP 1.1
	+ Compatability for release 1.1 
* 2014-0916: Release ? (MSD) for KSP 0.24.2
	+ No changelog provided 
* 2013-0725: Release 0.12 (Fel) for KSP 0.20.2
	+ Much Kudos to Razchek for finally slaying the Ragdoll Monster!
	+ I believe the rotation issues are 'almost fixed', they no longer walk on their sides while at the poles of the mun.
	+ Various 'animation related' bug fixes, (mostly, needing to reset the animation state)
	+ Tried to annotate the source a little more.
	+ Added a 'tighten up' box that should show up on only the followers, which makes small groups (I'd say 5 most) maintain some semblence of a formation better than the default;
	+ I need to add in some kind of "boxed bounds" which will make the final push to keep kerbals in line.
		- (It's like putting marbles in a small box, enough to fill it and they'll stay put as long as you don't shake it).
* 2013-07??: Release 0.11.3 (Fel) for KSP 0.20.2 NO BINARIES
	+ Okay, that SHOULD fix the bug. There SHOULD NOT be any freezing...
Kerbals face the player when "follow me" is clicked, even if fairly close (would like some kind of "wave" too)
	+ Some code trying to break up clusters > 3 but isn't going to be very successful.
	+ Source Cleaning and Misc...
* 2013-07??: Release 0.11.2 (Fel) for KSP 0.20.2 NO BINARIES
	+ Added in a "psuedo random movement" (actually, the kerbals just select a leader when the player doesn't say "I'm your leader")
	+ Could be expanded to any vehicle (i.e. rover, bases, debris), thoughts?
[and yes, this ignores "Stay Put" after a while... eventually will merge the personalities a bit... and still lacking pathfinding so a kerbal that gets trapped in the "collision mesh" I made will bounce around XD. Additionally I need to add in a distance weighting factor to encourage "small groups" but also allow for the "running across" actions (over time, the kerbals will just form 1 solid mass)]
* 2013-07??: Release 0.11.1 (Fel) for KSP 0.20.2 NO BINARIES
	+ And fixed the bounding issue... sort of.
* 2013-07??: Release 0.11 (Fel) for KSP 0.20.2 NO BINARIES
	+ Kerbals are "space aware" in that they'll move out of the way while following... inefficient algorithm makes them "twitch" a fair amount, but maybe that is okay (it looks like they're settling in).
	+ Changed the dll name and made it in a more "official format", so just be aware when replacing the original version.
* 2013-0701: Release 0.0 (Fel) for KSP 0.20.2
	+ Proof of Concept
