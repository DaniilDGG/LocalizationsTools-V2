# Localization Unity Tools
 Tools for creating localization in Unity

This is a localization tool created for the Unity engine.

# Install via UPM Package

First, install UniTask: https://github.com/Cysharp/UniTask?ysclid=lulo5ve1dr734166736#upm-package
After that, install via git URL this package: https://github.com/DaniilDGG/LocalizationsTools-V2.git

# How to use

To get started with this localization tool, you must set up the languages using the Localization/Language settings window.

Next, add the localizations you need using the Localization/Localization Settings window, where you will need to first come up with a unique identifier for the localization item, and then set localizations for all the selected languages.
After that, you can use the LocalizationInfo component, which receives localizations. Besides it, there is already a ready-made TMPTextLocalization that localizes text. To do this, you just need to enter a unique localization identifier.

Also, to change the current language - use LocalizationController.SwitchLanguage(int language number, from 0) Example: Demo Scene.
Also, the tool supports XLSX import and export, for editing localizations outside the Unity editor.
