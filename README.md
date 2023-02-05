## **Hey Update Optimizer**

Hey Update Optimizer is a tool to improve the performance of game runtime. This tool adjusts the update frequency of components in the game and distributing the processing load.

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
using UnityEngine
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
