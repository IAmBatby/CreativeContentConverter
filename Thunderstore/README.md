# CreativeContentConverter
A fork of HolographicWings's LethalSDK to follow developers to convert content created with LethalSDK to content built for Lethal Company and/or LethalLevelLoader.


# **How To Use**

## **Setting Up Your New Development Environment**


> 1. Create New Unity Project
>    * *Unity Version: 2022.3.9f1*
>     * *High Definition Render Pipeline*

> 2. Install LC Project Patcher by Nomnom
>    * *https://github.com/nomnomab/lc-project-patcher*

> 3. Run LC Project Patcher

## **Moving Your Previous Lethal Expansion Work To The New Environment**


> 1. Open up your old and new Unity projects in Windows File Explorer

> 2. Move all related folders From Your old project to your new project

> 3. Refresh your new project and manually confirm your content looks right.
>     * *More specifically confirm that things like scrap and moon prefabs still have relevant LethalSDK components and you still have LethalSDK ScriptableObject assets like ModManifests and Moons.*

## **Installing Creative Content Converter**


> 1. If if it doesn’t exist, Create a new folder called Plugins in `/Assets/LethalCompany/Tools/`

> 2. Add the latest version of **LethalLevelLoader** and all its dependencies in the `/Tools/Plugins/` folder

> 3. Locate the **LethalSDK.dll** found in the content you moved from the old project, For most people this will be in `/Assets/Plugins/`

> 4. Replace the *previous* **LethalSDK.dll** with the *new* **LethalSDK.dll** contained in the CreativeContentConverter release
>     * ***This is the part most likely to go wrong, To ensure Unity correctly understands your content is using the CreativeContentConverter version of LethalSDK instead of the original, you must ensure you replace the dll in the exact same place in the project***

> 5. Refresh your new project and manually confirm your content looks right.
>    * *More specifically confirm that things like scrap and moon prefabs still have relevant LethalSDK components and you still have LethalSDK ScriptableObject assets like ModManifests and Moons.*

## **Using Creative Content Converter**

> 1. Create new Conversion Settings by going to the top of Unity and selecting `/Creative Content Converter/Create New Conversion Settings/`

> 2. Open the tool window by going to the top of Unity and selecting `/Creative Content Converter/Lethal Expansion Conversion Tool/`

> 3. Select either Moon or Scrap depending on your use case, and add your Lethal Expansion Moon & Scrap assets to the list. You can also press populate to add every found asset in the project.

> 4. Press Convert

> 5. **Your content should now be converted.**

> 6. Go to the AssetBundles tab in the Conversion Tool and clear `"lem"`

> 7. Go into the AssetBundle Browser by going to the top of Unity and selecting `/Window/AssetBundle Browser/`

> 8. In the configure tab, open any collapsed bundles, if any bundles are named “lem”, right click and delete them

> 9. Hit the refresh button in the top right of the AssetBundle Browser window
