# Hey Update Optimizer v1

<p align="center">
    <a href="https://u3d.as/30Rv" alt="Unity 2020.1+"><img src="https://img.shields.io/badge/Unity-2020.1%2B-blue.svg" /></a>
    <a href="https://github.com/JahnStar/Hey-Update-Optimizer/blob/master/LICENSE" alt="License: CC BY 4.0"><img src="https://img.shields.io/badge/License-CC BY 4.0-brightgreen.svg" />
    <a href="https://jahnstar.github.io/donate/" target="_blank" rel="noopener" noreferrer alt="Support the Developer">
    <img src="https://img.shields.io/badge/Donate-☕-orange.svg"/></a></a>

<p align="center">Hey Update Optimizer is a tool to improve the performance of game runtime. This tool adjusts the update frequency of components in the game and distributing the processing load.</p>

<p align="center">The plugin is available on the <b>Unity Asset Store</b> for free: <a href="https://assetstore.unity.com/packages/slug/245930"><b>Click for add to your unity assets</b></a></p>

## **Features**

- Easy to use and implement in your project.
- Ability to adjust the update frequency of each component of the objects in the game.
- Ability to set the number of updates per frame.
- Ability to set the pooling ratio for updates.

## **Installation**

The tool can be found in the Unity Asset Store and on GitHub. To install, simply download and add the Hey Update Optimizer component to your Unity project.

## **Usage**

Hey Update Optimizer component is designed to be easy to use. Firstly, implement the ‘IHeyUpdate’ interface to the components you want to be affected. Then, add the "Hey Update Optimizer" component to a game object in the scene using the add component button and adjust the update frequency and pooling ratio settings to meet the needs of your game. The tool will automatically handle the rest.

Example component script:

```csharp
using UnityEngine;
using JahnStar.Optimization;
public class Example : MonoBehaviour, IHeyUpdate
{
	public int UpdatePerFrame { get => 1; }
	public void HeyUpdate(float deltaTime)
	{
		if (!gameObject.activeInHierarchy) return;
		// Your update code here
	}
}
```

## **Contributors**

Currently, there are no contributors.

## License

Shield: [![CC BY 4.0][cc-by-shield]][cc-by]

Hey Update Optimizer © 2023 by Halil Emre Yildiz is licensed under the
[Creative Commons Attribution 4.0 International License][cc-by].

[![CC BY 4.0][cc-by-image]][cc-by]

[cc-by]: http://creativecommons.org/licenses/by/4.0/
[cc-by-image]: https://i.creativecommons.org/l/by/4.0/88x31.png
[cc-by-shield]: https://img.shields.io/badge/License-CC%20BY%204.0-lightgrey.svg
