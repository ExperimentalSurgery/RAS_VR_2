# Haptic Interaction in Robotic Assisted Surgery (RAS)
"**Haptic Interaction Toolkit: A VR-Based Robotic Console for Experimental Investigation of Haptic Feedback in Robotic-Assisted Surgery**"
This project, *Haptic Interaction in RAS*, focuses on developing an XR simulation in Unity to explore and test various experimental setups. It aims to create and evaluate new methods for interacting with the patient’s body during robotic-assisted surgery.

The simulation is particularly valuable for researchers and developers interested in how haptic feedback influences the perception of virtual objects.

### Requirements

To get started with the project, you'll need the following hardware:

* **Meta Quest 3 Headset**
  [https://www.meta.com/de/quest/quest-3](https://www.meta.com/de/quest/quest-3)

* **3D Systems Haptic Device (Touch or Touch X)**
  [https://www.3dsystems.com/haptics-devices/touch](https://www.3dsystems.com/haptics-devices/touch)

**Abstract**
[To be completed after full paper is drafted: 200–300 words summarizing motivation, system, methods, example scenarios, key results, limitations, and availability of code/data.]

**Introduction**
RAS in advanced medical settings has transitioned from direct manual operation to digitally mediated control interfaces, which often result in a diminished or non-existent sense of touch for surgeons due to limited haptic feedback. This change presents significant technical and design challenges for replicating tactile experiences essential to surgical performance. To address this gap, specialized physical interfaces and Virtual Reality (VR) platforms are being developed to simulate and restore meaningful haptic cues.
As these technologies rapidly evolve, available tools vary in terms of interaction modes, feedback fidelity, and system integration. This article delineates the technical features and implementation workflow of the Haptic Interaction Toolkit, a VR-based robotic console designed for experimental investigation of haptic feedback in surgical contexts, and illustrates its use in representative surgical scenarios to support replication and further development.

1.1 Background and motivation

1.2 Objectives and contributions
(1) what the toolkit is (2) what is technically novel (e.g. console replica + VR twin + specific haptics), (3) what scenarios/experiments it enables, (4) availability of code, models, and build files.

2 Method
This section describes the hardware, software, and implementation details of the Haptic Interaction Toolkit, including physical console construction, haptic device integration, and VR application architecture.

2.1 System overview
High-level block diagram description: physical console + Touch devices + HMD + PC + Unity application + plugins + data logging.

2.2.1 Physical console design
The physical robotic console was designed with the aim of replicating the measurements from the da Vinci Xi surgical system console at Charité – Universitätsmedizin Berlin, Virchow-Klinikum. The console frames are fabricated from high-grade metal components, for which all laser-cut and 3D-printed parts must strictly adhere to specified tolerances to ensure structural stability and accurate haptic device positioning. Any deviation in the manufacture or assembly of these parts may induce spatial misalignments, which can compromise the overall fidelity of force and position feedback.
A comprehensive 3D digital twin of the physical console is implemented in the VR setup, maintaining millimeter-level spatial correspondence between the real and virtual reference points (see Fig. X). This portable station, equipped with lockable wheels, features two identical Touch devices mimicking the da Vinci controllers, as well as ergonomically contoured arm rests (see Figs. X–Y).
To guarantee steady visuo-haptic congruence, the system’s calibration protocol mandates that the physical layout (including console height, Touch device locations, and surface contours) is carefully aligned with its virtual counterpart before any experimental session. Proper fabrication and assembly of metal and 3D-printed parts are essential not only for stable device operation, but also for aligning tactile and proprioceptive feedback, ensuring that the user’s body position and exerted forces are reliably mirrored in the VR environment.
  Insert precise mechanical drawings and dimensions here, or reference supplementary CAD files.
  Insert bill of materials (BOM) as table or supplementary material.

2.2.2 Haptic devices and placement
The toolkit employs two 3D Systems “Touch” haptic devices, each providing 6-degrees-of-freedom positional sensing with 3-degrees-of-freedom force feedback. Each device features an adjustable stylus capable of transmitting haptic cues such as stiffness, viscosity, damping, force thresholds, sensitivity, positional accuracy, range of motion, grip strength, feedback gain, compliance, speed, response time, elasticity, and friction.
When installing these devices into the console, the lateral spacing between the two Touch devices on the physical console must be set to [XX.X cm], corresponding to [YY.Y units] in the Unity virtual environment, in order to preserve spatial congruence. This spatial congruence is essential, as any misplacement can critically affect the overall user proprioception in VR.
While the implementation of two identical devices can bring some advantages (e.g. code reuse and symmetric controller mappings), both devices are operated by different hands, which requires careful pre-calculation of workspace overlap, degrees-of-freedom, and potential collisions between the two kinematic chains.
Connection to the Unity simulation system is achieved via high-speed USB 2.0/3.0, supporting up to 2 kHz refresh rates for real-time data exchange and haptic rendering, depending on driver and plugin configuration.

2.2.3 VR headset and host computer
Hardware requirements for the reference setup include:
Host computer: AMD Ryzen 3950X CPU with Nvidia Geforce RTX 3090 GPU and 64 GB RAM. 
HMD: Meta Quest 3 (Meta Platforms Inc., USA, California), operated via appropriate Meta/OpenXR integration. 
Haptic devices: Two 3D Systems Touch devices (3D Systems Corporation, USA, North Carolina). 


2.3 Software architecture
2.3.1 Game engine and core libraries
The VR application is implemented using the Unity game engine (Unity Technologies Inc., version 6000.0.36f1). Tactile cues are rendered through the haptic devices using the Unity-compatible “Haptics Direct for Unity V1” plugin.
Additional SDKs and plugins include Meta’s Unity SDK and OpenXR plugins to maintain VR features (e.g. hand tracking, passthrough) from the Meta Quest 3 HMD. The 3D Systems Touch API/driver stack is used for low-level communication with the haptic devices.
Meta Quest SDK: https://developers.meta.com/horizon/develop/unity 
OpenXR integration within Unity: [Insert specific package versions].
3D Systems Haptic Device (Touch/Touch X): https://www.3dsystems.com/haptics-devices/touch 
Haptics Direct for Unity V1: https://assetstore.unity.com/packages/tools/integration/haptics-direct-for-unity-v1-197034 

2.3.2 Scene structure and interaction logic
Anna: Describe high-level Unity scene organization, prefabs, and interaction logic

2.3.2.1 Scene loader and main menu

2.3.2.2 Calibration scene (console alignment+Touch devices)

2.3.2.3 Task/scenario scenes (e.g. Settings 1, 2, 3 (?)
Anna: 
Describe how scenes are loaded and how state is passed.
Describe how Touch device transforms are mapped into Unity coordinates.
Describe how haptic materials and effects are configured.

2.3.3 Haptic rendering pipeline
Anna: Describe how forces are computed and sent to devices.
Include:
Representation of virtual interactions (rigid bodies, colliders, constraints).
Mapping of physical parameters (stiffness, damping, friction) to plugin parameters.
Update rates and threading (e.g. Unity frame vs. haptics loop).
Insert pseudo-code or code fragments if allowed; ensure code is clearly documented and human-readable.


2.4 Assets and virtual environment
The VR operating room and anatomical assets are derived from openly available or institutionally hosted 3D models:
Human base meshes: https://www.blender.org/download/demo-files/ 
Virtual OR: Charité university hospital OR model on Sketchfab. 
Abdomen anatomy model: https://sketchfab.com/3d-models/abdomen-anatomy-ed05d3b7b49b4014a09d7a9d62e4f421 
Liver model: https://bwgcloud.hu-berlin.de/d/b499550961b94e62b692/ 
Texturing tools: Substance Painter (Adobe Substance 3D Painter). 
Anna: Explain how assets are imported, scaled, and aligned with the physical console geometry.

2.5 Installation, setup, and reproduction
A step-by-step procedure for reproducing the toolkit

2.5.1 Software installation
Download and install the Unity project from the provided public repository (DOI/URI to be added). 
Install the 3D Systems Touch drivers and calibration tools. 
Install Haptics Direct for Unity V1 and import it into the Unity project. 
Install Meta Quest 3 / OpenXR Unity packages, configure XR plug-in management, and enable hand tracking and passthrough as needed. 
Build and deploy the application or run it in play mode with Quest Link / Air Link connection to the headset. 
Anna: Insert exact repository URL, branch name, Unity package versions, and platform settings.


2.5.2 Physical console setup
3D-print and/or machine physical console components using the supplied STL/OBJ and metal fabrication drawings. 
Console frame and table surface
Mounting bases for Touch devices
Liver and liver platform
Calibration platform
Pyramid end-tip of the controller
Arm rest components
Assemble the console following the construction guide provided as supplementary material (illustrated “IKEA-style” step-by-step diagrams). 
Mount the two Touch devices at the specified positions and orientations, ensuring the required spacing and height relative to the arm rests. 
Install and connect the Meta Quest 3 HMD and host PC according to the hardware schematic. 

2.5.3 Application startup and calibration
Upon startup, the user is guided through a calibration procedure that aligns the physical console with the virtual environment and calibrates the haptic devices.

2.5.3.1 Device calibration
2.5.3.2 Spatial alignment (real–virtual registration)
2.5.3.3 Verification (test scene with known targets)
Anna: Insert detailed step-by-step instructions, including screenshots, for: centering the stylus, aligning the virtual console, checking arm rest positions, etc...

3 Results
Examples of use (e.g. experimental scenarios or pilot tasks) and observed limitations of the toolkit...?

3.1 Example scenarios
[Describe each experimental or demonstrator scenario supported by the toolkit, focusing on how the system is used rather than study results.]

Reference representative screenshots and demo videos (calibration, usage) hosted at: https://bwgcloud.hu-berlin.de/d/4a507cf93cee46f996ea/?p=%2F&mode=list 

3.2 Performance and usability observations
  
3.3 Limitations observed
Anna: List current limitations discovered during use: e.g. device workspace constraints, sensitivity to misalignment, performance bottlenecks, VR tracking drift, hand–controller mode switching....

4 Discussion
4.1 Design choices 
[Discuss design decisions (e.g. using 3D Systems Touch vs. other devices, Meta Quest 3 vs. tethered HMD, metal console vs. full 3D-print) and how they affect usability, fidelity, and reproducibility.]
4.2 Scalability and extensibility
[Discuss how the toolkit can be extended: e.g. different organs/tasks, additional sensors, multi-user setups, integration with real robotic systems, or deployment in other institutions.]
4.3 Current limitations and future work
[Summarize main technical limitations and outline specific future improvements: e.g. optimized console weight, improved comfort, wireless haptics, more advanced haptic shaders, better calibration workflows.]

5 Code, data, and architecture
5.1 Code availability and repository structure
[Provide repository DOI/URL, license, and a short description of directory structure:]
/Assets/ (Unity assets, scenes, scripts)
/Haptics/ (plugin configuration, haptic materials)
/ConsoleCAD/ (CAD files, STL/OBJ)
/Docs/ (assembly guide, calibration manual)
/Data/ (optional example datasets or logs)
Explain how to:
Build the Unity project.
Configure haptic materials.
Enable/disable optional modules (e.g. experimental logging).
5.2 Data structures and logging
[Describe what data is collected during use: e.g. 6-DoF pose, forces, button presses, timestamps, task-specific metrics, VR tracking data.]
Include:
Data schema (variable names, types, units).
File formats (e.g. CSV, JSON).
Any anonymization/pseudonymization measures.
5.3 Privacy and ethical considerations in VR
[Explain how the toolkit handles data protection and privacy, especially if used with human participants.]
Include:
What identifying information is or is not stored.
Recommendations for ethical approval and informed consent when using the toolkit in user studies.
Storage and access recommendations.

6 Contribution guidelines
[Describe how others can fork and extend the project.]
Include:
Repository location and branching model.
How to submit bug reports and pull requests.
Coding style and documentation expectations.
Dependency management (Unity version, plugin versions) and typical troubleshooting steps (driver issues, XR plugin conflicts, calibration drift).
Possible bullets:
Bug fixes (where to file issues, how to reproduce).
Adding scenarios or haptic effects.
Extending support to other haptic devices or HMDs.

7 Figures, videos, and supplementary material
List figure types and placeholders:
Fig. 1–X: Physical console photographs and CAD renderings.
Fig. X–Y: VR environment screenshots showing console, OR, and tasks.
Fig. X: Software architecture diagram and data flow.
Supplementary videos: device calibration, user interaction from first-person and third-person perspectives. 
Link or reference external media repository (with DOI if possible):
https://bwgcloud.hu-berlin.de/d/4a507cf93cee46f996ea/?p=%2F&mode=list 

8 Funding, acknowledgments, and license
8.1 Funding and acknowledgments
This work was supported by the Cluster of Excellence “Matters of Activity. Image Space Material” funded by the Deutsche Forschungsgemeinschaft (DFG, German Research Foundation) under Germany’s Excellence Strategy – EXC 2025.
[Add acknowledgments for collaborators, labs, and institutional support as appropriate.]
8.2 Authors and contributors
Example list (to be finalized with CRediT roles):
Moritz Queisner
Zeynep Akbal
Igor M. Sauer
Anna Yadygina
Christopher Remde
Julia Blumenthal
[Add roles such as conceptualization, software, hardware, validation, writing.]
8.3 License
[Specify license(s) clearly:]
Source code: MIT License (or alternative, as decided).
3D models and textures: [e.g. CC BY 4.0 / CC BY-NC-SA, depending on asset permissions].
Documentation and figures: [license].
Ensure compatibility with the licenses of reused assets from Blender, Sketchfab, and other sources.

9 References
[Standard Frontiers reference style, including references to:
RAS and haptic feedback in surgery.
VR haptics and prior wearable/fo












