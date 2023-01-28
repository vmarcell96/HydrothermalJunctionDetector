# HydrothermalJunctionDetector

### Description

This is a small utility application as an extension for a submarine control system for it to safely cross the
ocean. Hydrothermal vents tend to form lines. Where two or more of these lines cross they constantly produce
large, opaque clouds. For safety reasons these are to be avoided. Most of the hydrothermal lines are already
known. They are provided via an input text file containing the coordinates of the start and end points of each line.
The application should calculate - based on the hydrothermal lines of the input file - where the dangerous points for
the submarine are located. A user interface (command line) shall allow to specify the input text file, which provides
the basic data for the calculation. The file shall be analyzed, validated and parsed to retrieve valid hydrothermal
lines for the calculation (details below). Further the application should calculate the crossing points of the
hydrothermal venting lines and report it to the user on the command line and additionally write it into an output file.
