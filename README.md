# MPCIC

![Test](https://github.com/plule/MPCIC/workflows/Test/badge.svg)
![Publish](https://github.com/plule/MPCIC/workflows/Publish/badge.svg)

MPCIC is a basic command line software generating MPC keygroup instruments.

It takes as an input a list of .wav files named after their root note, and create an xpm file that can be loaded in the modern MPCs.

## Usage

 - Group the samples you want to use in the same folder
 - Download the latest version from https://github.com/plule/MPCIC/releases
 - Extract the archive. There is no installation process.
 - Open a terminal and run <path/to/MPCIC-{os}/MPCIC.Cli> <path/to/the/sample/folder>

MPCIC is not properly signed for MacOS. In order to run the software you need to disable Gatekeeper with `sudo spctl --master-disable`. Once you generated your programs, re-enable it using `sudo spctl --master-enable`.

## Example of supported sample naming scheme

Using note name:

 - MellotronCello-A2.wav
 - MellotronCello-D3.wav
 - MellotronCello-A3.wav
 - MellotronCello-D4.wav
 - MellotronCello-A4.wav
 - MellotronCello-D5.wav

Using midi number:

 - THMB40.WAV
 - THMB43.WAV
 - THMB48.WAV
 - THMB52.WAV
 - THMB55.WAV
 - THMB60.WAV
