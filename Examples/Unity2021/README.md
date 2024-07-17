# Unity Plugin

This is a barebones example plugin for Unity 2021 using RmlUi.Net. All data is read from `StreamingAssets/rml` and a number of things are still hardcoded but it's a good starting point.

## Requirements
- Universal RP
- Input System

## Usage
Build RmlUi.Native and RmlUi.Net (in that order) and place the resulting output from RmlUi.Net's build process within the `plugins` directory of the plugin.

Place this entire folder within a subfolder of the `Assets/Plugins` directory of your Unity project.