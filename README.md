# HydrothermalJunctionDetector

## Description

This is a small utility application as an extension for a submarine control system for it to safely cross the
ocean. Hydrothermal vents tend to form lines. Where two or more of these lines cross they constantly produce
large, opaque clouds. For safety reasons these are to be avoided. Most of the hydrothermal lines are already
known. They are provided via an input text file containing the coordinates of the start and end points of each line.
The application should calculate - based on the hydrothermal lines of the input file - where the dangerous points for
the submarine are located. A user interface (command line) shall allow to specify the input text file, which provides
the basic data for the calculation. The file shall be analyzed, validated and parsed to retrieve valid hydrothermal
lines for the calculation (details below). Further the application should calculate the crossing points of the
hydrothermal venting lines and report it to the user on the command line and additionally write it into an output file.

## Features

- Read text file
- Parse venting data
- Only accepts files, where the lines have the hydrothermal venting line format
- If there is a line not following the hydrothermal venting line format the parsing stops
- Neither horizontal, vertical or diagonal (45°) lines are skipped
- Progress bar
- Process is cancellable when progress bar is displayed

## Run Locally

##### Prerequisites

- Microsoft Visual Studio to run application

Clone the project and navigate to the project folder

```bash
  git clone git@github.com:vmarcell96/HydrothermalJunctionDetector.git
  cd .\HydrothermalJunctionDetector
```

Build the project

```bash
  dotnet build
```

Navigate to built project's folder

```bash
  cd .\bin\Debug\net6.0
```

Run the exe file

```bash
  HydrothermalJunctionDetector.exe
```

## Bugs

- After the parsing you have to keypress two times because the background task which listens to keypress for cancelling the parsing is runs in the background

## Usage

- Type "default" when asked for input file if you want to use the default file (InputFileLineSegments.txt)
- When the progress bar shows that is possible to cancel the process
- Type "default" when asked for output directory if you want to output into the root directory, files name will be:  CrossingPoints-ID.txt
- Currently you can only select output directory, cannot use custom name for the output file

## Roadmap

- Fix keypress listener background thread bug
- Selecting custom output file name
- Updated UI




