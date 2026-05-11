# Neural Network Activity Simulation: Conway's Game of Life in Unity

## Overview

This game is a simulation inspired by Conway's Game of Life, modeling the behavior of neurons in a neural network. The simulation features cells on a grid that activate and connect based on specific rules, divided into functional brain regions to mimic neurological activity variations.

## Objective

The project is not just about replicating the classic cellular automaton but also about extending it by introducing custom themes, unique rules, and environmental modifications to analyze their effects on cell behavior. 

## Task

1.  Implement Conway's Game of Life and explore its properties using Unity (or any other game engine agreed from the start with the teacher). 
2.  Upload the game on [https://itch.io/](https://itch.io/) 

## Theoretical Notes

John Conway's Game of Life is a cellular automaton, which is supposed to produce patterns that resemble living organisms. </br>It was developed by the mathematician John Conway in the 1970s. The Game of Life is played on an infinite two-dimensional rectangular grid of cells. Each cell can be either alive or dead. The status of each cell changes each turn of the game (also called a generation) depending on the statuses of that cell's neighbors.  

These simple rules are as follows:

1.  If the cell is alive, then it stays alive if it has either 2 or 3 live neighbors; 
2.  If the cell is alive and has fewer than two living neighbors, it dies from underpopulation;
3.  If the cell is alive and has more than three living neighbors dies from overpopulation; 
4.  If the cell is dead, then it springs to life only in the case that it has 3 live neighbors;


## Implementation 

Our game models the behavior of neurons in a neural network, where cells on a grid fire and connect based on specific activation rules. </br>The simulation divides the grid into functional brain regions, modifying behavior in different zones to reflect neurological activity variations.

The primary mechanics include:

**Base Rules (Inspired by Conway's Game of Life)**

Each cell (neuron) can be in one of two states: 

1.  Active (Firing)
2.  Inactive (Resting)

The activation of a neuron depends on the number of active neighbors. 

**Zone-Based Modifications**

The grid is divided into four zones: 

1.  Two standard zones that follow the original Game of Life rules (top-right and bottom-left)
2.  Two special zones with modified rules to simulate different neural activity: 

    **(a) Sensory Cortex (Top-Right):**

    - Neurons have an increased activation probability. 
    - A neuron can activate if it has 2 or more active neighbors. 
    - A living neuron survives if it has one, two, or three living neighbors. 
    - Deactivation occurs at a higher neighbor threshold.

    **(b) Hippocampus (Bottom-Left):**

    - Neurons have a decreased activation probability.
    - A neuron needs at least four active neighbors to activate. 
    - A living neuron survives only if it has exactly two living neighbors. 
    - Deactivation occurs at a lower neighbor threshold, making this zone more stable. 


## User Interaction Interface

The game provides a control panel at the upper-right corner of the screen, allowing users to interact with the simulation. The panel includes:

1.  Start / Pause Button: Begins or pauses the simulation. 
2.  Simulation Speed Slider: Allows users to speed up or slow down the simulation. 
3.  Generation Counter: Allows users to choose the number of generations and displays the current generation number as the simulation progresses.  
4.  Infinity Button: Allows users to specify the number of generations as being infinite.  

## Grid Interaction

Users can directly manipulate the grid by:  

1.  Drawing Patterns: Clicking on individual cells toggles them between active and inactive states, allowing users to create custom initial neuron arrangements. 
2.  Zooming and Panning: Support for mouse wheel zooming and click-and-drag panning for easier navigation of large grids.  

## Simulation Analysis

The game operates on a step-based update cycle, where each frame represents a new generation. </br>The update process follows these steps: Each neuron checks its current state and the number of active neighbors. Then, the activation rules are applied. The grid updates, displaying the new neuron states, and the process continues, allowing emergent patterns to form. 

The zones behave in the following ways:

* Standard zones behave exactly like the original Game of Life, and they have balanced neuron activation and deactivation.  
* The Sensory Cortex Zone has faster, more chaotic activation. Larger active clusters form due to relaxed activation rules, and this zone practically never dies. 
* The Hippocampus Zone is more stable, resistant to activation. Smaller, isolated active groups form due to higher activation thresholds. It dies the fastest if the patterns allow it to. 

Modifying standard cellular automata rules creates a neural-like simulation. Different brain zones influence neural activity in expected ways.

## Conclusions

Through this project, we demonstrated how modifying the standard rules of a cellular automaton can simulate complex systems, such as neural networks, and how different regions with specific rules can influence overall behavior. This approach provides valuable insights into modeling and understanding complex biological systems using computational simulations.  

## Bibliography

1.  [https://maxim-isacescu.itch.io/game-of-life](https://maxim-isacescu.itch.io/game-of-life) 
2.  [https://github.com/M1XaM/GameOfLife](https://github.com/M1XaM/GameOfLife) 
3.  [https://pi.math.cornell.edu/~lipa/mec/lesson6.html](https://pi.math.cornell.edu/~lipa/mec/lesson6.html) 
4.  [https://plus.maths.org/content/maths-minute-conways-game-life](https://plus.maths.org/content/maths-minute-conways-game-life) 