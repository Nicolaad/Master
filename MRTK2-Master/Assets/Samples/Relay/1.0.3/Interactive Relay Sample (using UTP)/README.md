# Interactive Relay using UTP Sample

This is a Unity package sample to demonstrate how to initialize and persist a Relay allocation over the Unity Transport Package layer (UTP).
Note that Relay allocations can alternatively be utilized over other 3rd party transport layer services instead of UTP, but such integrations are not shown in this sample.

## Prerequisites

Your current Unity project needs to be initialized on the Unity Dashboard with Relay enabled. Follow the below steps to do so:
* Login to [Unity Dashboard](https://dashboard.unity3d.com/).
* Create a project or open a pre-existing project.
* Navigate to `Relay` (under `Multiplayer` in the left navigation panel). Follow the `Getting Started` steps and make sure `Relay` is enabled (`Relay On` toggle).
* Open project in Unity 2020.3.12f1+.
* Link the Unity project to your project and organization (in `Project Settings > Services`).

## Using this sample

Open the project with the Unity editor (2020.3.12f1+), open the `relay-sdk-test (utp)` scene.
When entering PlayMode, the sample will automatically sign in the current user with anonymous sign-in. This will be displayed as a Player ID. The allocation's region is automatically assigned.
You may either set the client to `Host` (Listen server) which will connect to the client immediately after the server has started, or as a `Client`.

To test the connection between multiple instances, build and run a separate executable that loads the `relay-sdk-test (utp)` scene. 
Set one instance to run as host and other instances to run as client. One of your instances can be the editor running in PlayMode.
Connect using the `JOIN CODE` displayed on the host instance. On successful connection, the player Allocation ID will be generated.
