# Haptic Interaction in Robotic Assisted Surgery (RAS)

This project, *Haptic Interaction in RAS*, focuses on developing an XR simulation in Unity to explore and test various experimental setups. It aims to create and evaluate new methods for interacting with the patient’s body during robotic-assisted surgery.

The simulation is particularly valuable for researchers and developers interested in how haptic feedback influences the perception of virtual objects.

---
## Installation

### Requirements

#### Software
* **Unity  version: 6000.0.58f2**
* **Haptic Plugin: Haptics Direct for Unity V1 (https://assetstore.unity.com/packages/tools/integration/haptics-direct-for-unity-v1-197034?srsltid=AfmBOooI1uYz4nglnz78gmfS9Hods_pZBOLKbmEDijF3nTlSZu74s8C-)**
* **Meta SDK**
* **Meta Horizon Link**
* **Haptic Device Drivers**

#### Hardware
* **Meta Quest 3 Headset**
  [https://www.meta.com/de/quest/quest-3](https://www.meta.com/de/quest/quest-3)

* **3D Systems Haptic Device (Touch or Touch X)**
  [https://www.3dsystems.com/haptics-devices/touch](https://www.3dsystems.com/haptics-devices/touch)

### Setup
* **Clone this repository with Git**
* **Open Haptic Drivers to calibrate touch devices**
* **Open Meta Horizon Link  and connect you Meta Quest 3 Headset to your PC**
* **Open the project with Unity 6000.0.58f2**

## Citation
## License

flowchart TB

%% ======================
%% Windows PC Layer
%% ======================
subgraph PC["Windows PC (PCVR Host)"]

  subgraph Unity["RAS XR Application (Unity Game Engine)"]
    MetaSDK["Meta XR SDK (Unity)"]
    HapticsDirect["Haptics Direct for Unity v1 Plugin"]
    HapticlabsAPI["Hapticlabs Unity API"]
  end

  MetaRuntime["Meta XR Runtime (OpenXR)"]
  HorizonLink["Meta Horizon Link (PC App)"]

  OpenHaptics["OpenHaptics (3D Systems Runtime)"]
end

%% ======================
%% Hardware Layer
%% ======================
Quest["Meta Quest 3\n(XR Headset)"]
Touch["Touch Haptic Devices (x2)"]
Hapticlabs["Hapticlabs DevKit\n(Satellite + 2× Actuators)"]

%% ======================
%% Connections
%% ======================

%% XR Pipeline
MetaSDK --> MetaRuntime
MetaRuntime --> HorizonLink
HorizonLink --> Quest

%% Kinesthetic Haptics Pipeline
HapticsDirect --> OpenHaptics
OpenHaptics --> Touch

%% Tactile Haptics Pipeline
HapticlabsAPI -->|Serial| Hapticlabs


