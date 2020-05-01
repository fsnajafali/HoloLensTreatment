# How to Get the Application Contents Onto Your Computer
The contents of this document contain all information needed to deploy the application to the HoloLens from your computer from scratch.

## Getting Set Up With Unity
This project uses Unity versioned at `LTS 2018.4.12.f1`.

LTS stands for Long Term Support, indicating that this build is used to ensure future stability.

Install `Unity LTS 2018.4.12.f1` here by downloading the Unity Editor Download Assistant for your operating system:
https://unity3d.com/unity/qa/lts-releases


### Install Vuforia While Using the Download Assistant
When installing Unity via the correct Unity Editor Download Assistant, you will be prompted with an option to install specific Unity packages, or components.

This is how Vuforia is installed into the project. Vuforia provides the foundational functionality of our Augmented Reality scene.

At the package or component install screen, select `Vuforia Augmented Reality Support` as a package to install with your Unity installation.


## After Installing Unity, Install the Project Locally
### Create an Empty Unity Project
Create a new Unity Project using the 3D template.

Open the project.


### Install the Microsoft Mixed Reality Toolkit
Microsoft's Mixed Reality Toolkit (MRTK) serves as a foundational package for all of our scenes, providing HoloLens integration to user input.

We used MRTK at version `2.2.0` designed for Unity in our project.

Download MRTK versioned at `2.2.0` in the form of an importable Unity package via direct download here:
https://github.com/microsoft/MixedRealityToolkit-Unity/releases/download/v2.2.0/Microsoft.MixedReality.Toolkit.Unity.Foundation.2.2.0.unitypackage .

Double-click the unitypackage to import it.

Wait for it to import.

If `MRTK Project Configurator` comes up, hit `Apply`.

Wait til it's done.

Go to `File > Build Settings...`.

Click `Universal Windows Platform`.

Set `Target Device` to `HoloLens`.

Click `Switch Platform`.

Wait.

If `MRTK Project Configurator` comes up again, hit `Apply`. Wait.

Close Unity.


### Replace the Empty Project Configured With MRTK With Our Project

Open the project folder in your system's file explorer, typically found at `This PC/Documents/[Your Project Name]`

Open the Assets folder (`[Your Project Name]/Assets`) and delete the `Scenes` folder.

Go back to the project folder (`[Your Project Name]`) and rename `Assets` to `Assets_`.

Open a shell from the project folder and run `git clone https://github.com/Silidos/ABB2020.git Assets` or use your preferred method of pulling the files from GitHub.

Once that's done, drag everything from `Assets_` into `Assets` and then delete `Assets_`. `Assets` is the file path that Unity will read, so here we move the contents of our project from a temporary folder into the permenant one.


### Finalize Project Dependencies
Open the project in Unity.

Go to the `Scenes` folder and double-click `MainMenu`.

A dialog about TextMeshPro should pop up, click `Import TMP Essentials` and then close the window when it's done.

Open the `Scenes` folder in Unity.

Open `File > Build Settings...`.

Drag all the `.scene` files from the `Scenes` folder into the `Scenes In Build` box.


Save the project.


You should now be good to go!

Note: Do NOT commit any of the `MixedRealityToolkit`, `MixedRealityToolkit.Providers`, `MixedRealityToolkit.SDK`, `MixedRealityToolkit.Services`, or `TextMesh Pro` folders, because they WILL break the project.
