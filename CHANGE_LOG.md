# EVA Follower /L Unleashed :: Change Log

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
