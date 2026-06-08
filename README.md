<img width="4100" height="2500" alt="Figure 1(A),(B)" src="https://github.com/user-attachments/assets/ee398805-f8e3-4428-a2df-1a2d740f26a1" />

# Haptic Interaction Toolkit (HIT) for Robotic-Assisted Surgery

An open-source, Unity-based mixed-reality platform for experimental investigation of haptic feedback in robotic-assisted surgery (RAS).

HIT combines a physical console, dual force-feedback devices, and a configurable software pipeline to support controlled visuo-haptic experiments. It is designed as a low-cost research testbed that enables systematic manipulation of haptic variables without requiring access to clinical robotic systems.

This repository contains the reference Unity project, three example scenarios, calibration tools, and the configuration files needed to reproduce the setup described in the associated publication.

> **Status:** Technical proof-of-concept. The platform provides the infrastructure to implement and explore haptic interaction strategies; validated user-study protocols and quantitative evaluation are planned as next steps.

---

## Table of Contents

- [Overview](#overview)
- [Requirements](#requirements)
- [Repository Structure](#repository-structure)
- [Getting Started](#getting-started)
- [Hardware Setup](#hardware-setup)
- [Calibration](#calibration)
- [Experimental Scenarios](#experimental-scenarios)
- [Data Logging](#data-logging)
- [Limitations and Known Issues](#limitations-and-known-issues)
- [Citation](#citation)
- [License](#license)

---

## Overview

Robotic-assisted surgery typically deprives surgeons of haptic cues. HIT provides a reproducible experimental infrastructure for investigating how different haptic feedback strategies affect perception and task performance in surgical scenarios. The system integrates:

- A **physical console** modeled after the Intuitive da Vinci Xi surgeon console
- **Two 3D Systems Touch** grounded force-feedback devices
- A **Meta Quest 3** head-mounted display for mixed-reality visualization
- A **Unity-based software pipeline** with real–virtual registration, configurable haptic rendering, and data logging
- An optional **Hapticlabs** vibrotactile feedback channel

Three reference scenarios demonstrate the toolkit's capabilities: liver stiffness discrimination, ureter boundary protection, and fatty tissue dissection with multimodal haptic cues.

---

## Requirements

### Hardware

| Component | Specification |
|---|---|
| Head-mounted display | Meta Quest 3 (PC-VR mode via Meta Horizon Link / OpenXR) |
| Meta Quest 3 Stylus Tip | 3D‑printed tip attached to the bottom nub of the Meta Quest 3 right controller, based on the open‑source model: [Meta Quest 3 Stylus Tip – Thingiverse](https://www.thingiverse.com/thing:6536226) |
| Haptic devices | 2× 3D Systems Touch or Touch X, mounted with ~30 cm lateral spacing |
| Host PC (reference config) | Windows 10/11, AMD Ryzen 3950X (or comparable), Nvidia RTX 3090 (or comparable dedicated GPU), 64 GB RAM |
| USB | High-speed USB 2.0/3.0 ports for both Touch devices |
| Vibrotactile feedback (Scenario 3 only) | Hapticlabs sensor-actuator DevKit |

### Software

| Component | Version / Source |
|---|---|
| Unity | 6000.0.58f2 (Windows target) |
| Haptic drivers | [3D Systems Touch drivers](https://www.3dsystems.com/haptics-devices/touch) |
| Haptic plugin | [Haptics Direct for Unity V1](https://assetstore.unity.com/packages/tools/integration/haptics-direct-for-unity-v1-197034) |
| VR integration | Meta / Oculus Unity SDK + OpenXR packages |
| PC-VR link | [Meta Horizon Link](https://www.meta.com/quest/setup/) (wired) or Air Link |
| Vibrotactile (optional) | [Hapticlabs Unity package](https://docs.hapticlabs.io/integrations/unity/) + [Hapticlabs Studio](https://www.hapticlabs.io/download) |

---

## Repository Structure

```
├── Assets/              # Unity assets: scripts, scenes, materials, models
├── Calibration/         # Calibration platform resources and reference data
├── Packages/            # Unity package manifest
├── ProjectSettings/     # Unity project settings
├── Screenshots/         # Reference images
├── RAS_VR_2.slnx        # Visual Studio solution file
└── README.md
```

---

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/ExperimentalSurgery/Haptic-Interaction-Toolkit-in-Robotic-Assisted-Surgery.git
```

### 2. Install drivers and prerequisites

- Install the **3D Systems haptic device drivers** and verify that both Touch devices are recognized by the OS.
- Install **Meta Horizon Link** and confirm the Quest 3 is available in PC-VR mode.
- If using Scenario 3, install **Hapticlabs Studio** and the Hapticlabs Unity package.

### 3. Open the project in Unity

- Launch **Unity Hub** and click **Open**.
- Select the cloned project folder.
- When prompted, select **Unity 6000.0.58f2**.
- After loading, verify that:
  - the **Haptics Direct** plugin is present and enabled (check `Window > Package Manager` or the Assets folder);
  - **Meta / OpenXR** packages are installed without errors in the console.

---

## Hardware Setup

### Haptic devices

- Mount both 3D Systems Touch devices on the console frame with approximately **30 cm lateral spacing** between end-effectors. This spacing corresponds to 0.30 units in the Unity coordinate system and is critical for spatial congruence between physical and virtual workspaces.
- Connect both devices to the host PC via USB 2.0/3.0.
- Run the 3D Systems diagnostic utility to verify device connectivity and calibration.

### Meta Quest 3

- Connect the Quest 3 to the PC via cable or Air Link.
- Confirm that the headset is available as an **OpenXR runtime** in Unity (`Edit > Project Settings > XR Plug-in Management`).

### Hapticlabs DevKit (Scenario 3 only)

- Mount the voice-coil actuator on a 3D-printed wristband and the sensor elements on the dorsum of the user's hand.
- Connect the Hapticlabs hardware via serial and configure it in **Hapticlabs Studio** or via Satellite.
- In Unity, add the **Hapticlabs Manager** prefab to the scene and set the connection mode (TCP for Studio, serial for Satellite).

---

## Calibration

Calibration aligns the physical console, haptic devices, and virtual environment. It is the mandatory first step before accessing any scenario.

### Procedure

1. **Launch the application.** The calibration scene loads automatically.
2. **Stage 1 — Console and phantom alignment:** Using the right Quest controller, place four virtual reference points into the corresponding physical sockets on the calibration platform. This establishes global spatial alignment between real and virtual environments.
3. **Stage 2 — Haptic device alignment:** Using the right and left Touch styluses, align four reference points per device with the corresponding virtual markers. This ensures correspondence between physical and virtual tool positions.
4. The alignment transformation is computed using the **Kabsch algorithm** on the collected point pairs.
5. After successful calibration, the main menu loads and experimental scenarios become available.

**Duration:** ~1–2 minutes. Can be performed by non-expert users after brief familiarization.

**Sources of residual error:** Reference-point placement accuracy, variations in how firmly devices and phantoms are seated, and manufacturing tolerances of 3D-printed components. Larger deviations may appear near phantom edges or highly curved regions.

---

## Experimental Scenarios

After calibration, the main menu allows users to launch the three reference scenarios:

### Scenario 1 — Liver stiffness discrimination

Participants palpate a virtual liver model using the Touch devices. The scenario progresses from mixed-reality mode (physical liver phantom present) to full VR mode with haptic feedback, enabling discrimination between soft (healthy parenchyma) and rigid (tumour) regions.

**Focus:** Kinesthetic stiffness cues, visuo-haptic integration, resection margin assessment.

### Scenario 2 — Ureter boundary protection

Participants navigate a virtual instrument near the ureter. A graded haptic safety zone renders increasing friction as the stylus approaches the protected structure.

**Focus:** Haptic spatial boundaries, force-based proximity feedback, situational awareness near critical anatomy.

### Scenario 3 — Fatty tissue dissection with vibrotactile cues

Participants dissect a semi-transparent fat layer to reveal underlying anatomy. A forearm-mounted Hapticlabs actuator delivers vibrotactile cues signaling instrument activation state, complementing continuous force feedback from the Touch devices.

**Focus:** Multimodal haptic feedback (kinesthetic + vibrotactile), tool-state awareness, distributed cue integration.

**Requires:** Hapticlabs DevKit.

---

## Data Logging

The project includes scripts for logging key variables during experimental sessions, including tool positions, applied forces, and event markers. Log files are written to a project-specific directory within the Unity project.

These logging routines can be extended to capture additional measures (task time, error metrics, subjective ratings) for formal user studies.

---

## Limitations and Known Issues

- **Proof-of-concept status.** The platform does not yet include completed user-study protocols or quantitative validation data.
- **Simplified tissue models.** Stiffness and damping values are qualitative cues, not biomechanically calibrated parameters.
- **Collider complexity.** Haptic rendering in Unity is collider-based. High-polygon, non-convex anatomical meshes may cause unstable force output and require simplified colliders.
- **Proprietary SDK dependency.** The 3D Systems Haptics Direct plugin operates as a closed-source component. Error handling and low-level parameter tuning are limited to exposed SDK interfaces.
- **Registration accuracy.** Residual physical–virtual misalignment may occur near phantom edges or curved regions. Consider these tolerances when designing spatially precise tasks.

---

## Citation

If you use HIT in your research, please cite:

**Associated manuscript (in review):**

> Akbal Z, Yadygina A, Remde C, Blumenthal J, Pratschke J, Sauer IM, Queisner M. *Haptic Interaction Toolkit*: A Mixed Reality-Based Robotic Console for Experimental Investigation of Haptic Feedback in Robotic-Assisted Surgery. Frontiers in Virtual Reality (in review).

**Related publications:**

> Akbal Z, Daneshgar A, Morgül MH, Moosburner S, Pratschke J, Sauer IM, Queisner M (2025). Exploring the Need for Haptic Feedback and Improved Communication in Robotic-Assisted Surgery: A Surgeon-Centered Survey. *IEEE Access* 13: 197889–197898. [doi:10.1109/ACCESS.2025.3633553](https://doi.org/10.1109/ACCESS.2025.3633553)

> Akbal Z, Sauer I, Yadygina A, Remde C, Blumenthal J, Queisner M (2026). Haptic Interaction Toolkit for Robotic-Assisted Surgery. In: *International XR-Metaverse Conference 2025*. Springer Nature, Chapter 32, pp. 230–243. eBook ISBN 978-3-032-11983-4. [Book info](https://link.springer.com/book/9783032119827#bibliographic-information)

---

## License

This repository is distributed under the [Creative Commons Attribution 4.0 International (CC BY 4.0)](https://creativecommons.org/licenses/by/4.0/) license.

Reuse, adaptation, and distribution are permitted provided appropriate credit is given to the original authors.

---

Developed at the [Digital Surgery Lab](https://experimental-surgery.de/digitalsurgery/), Department of Surgery, Experimental Surgery, Charité – Universitätsmedizin Berlin, as part of the Cluster of Excellence [Matters of Activity](https://www.matters-of-activity.de/), Humboldt-Universität zu Berlin.
