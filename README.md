# Prompt Generator #
A Random Character Prompt generator within the Godot Game Engine - Version 3.2 Mono.

The primary purpose of this generator is to be utilized within timed drawing intervals.
The generator boasts the ability to randomly generate a 'prompt' which consists of
a list of features/characteristics categories that the user can select. With this prompt the user
can draw their interpretation of the character.

## Capabilities / Advantages ##
- Variablility - Creating prompts based on user selected categories.
- Timed - Automatic timed intervals to insight quick creative drawings.

## Task List: ##
- [ ] Expand the current database of categories and prompts.
- [ ] Update the user interface styling.
- [ ] Investigate the usage of machine learning [dataset](https://quickdraw.withgoogle.com/data) to auto generate sketches from prompts.

## Detailed Description ##
The prompt generator can be utilized within only a few steps.

1. Starting the prompt generator.
2. Selecting at least **two** categories.
3. [Optional] Selecting timed checkbox and setting the time interval for timed prompts.
4. Selecting generate to begin prompt generation.

## Included ##
* Primary Scrips, Assets, Imports, Scenes
* Built Executable Project ([here](/_Builds/Build_1.0/))

## Example Imagery ##

#### Category Selection ####
Category selection currently requires at least two categories to be selected at one time in order to generate prompts.

![alt text](/Category_Selection_Example.png)

#### Generated Prompts ####

![alt text](/Generated_Prompts_Example.png)

#### Editing the Prompts Database ####
Currently the user only has the ability to add or remove the prompts from the prompt categories or reset to defaults.
Implementing adding or removing categories may or may not be added in future versions.

![alt text](/Editing_Database_Example.png)
