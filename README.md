# Peripheral Vision Experiment (VR Project)

This project is a VR application designed to study and demonstrate aspects of peripheral vision. It specifically focuses on peripcolour perception, light perception, and motion detection under different lighting conditions . The project leverages Unity's XR Interaction Toolkit and has been tested on the Meta Quest 2.

---

## Features

### Main Menu
- The application starts with a *Main Menu* scene, allowing users to select one of four experiments to explore. There is a brief introduction of the project in this scene, together with 4 entrance linked to four individual scenes.
![WhatsApp Image 2024-12-15 at 17 25 57_ba3ec23b](https://github.com/user-attachments/assets/fbe0db94-2a2a-4fcf-8d35-2ec7a3d582fe)


### Experiments
There're four scenes to test features of peripheral vision. Each of the scene include a spherical grid of balls, different behaviours are applied to the grid in different scenes to assess peripheral vision characteristics. Instructions also included in each of the scene.

1. *First Scene:  Peripheral Colour Detection*
   - Description: In this scene, a  ball will be selected randomly to change it's colour each time. It can be at the centre of the sphere, or at the edge of the sphere, which require player to detect with peripheral vision.
   - Goal: To test peripheral vision can not catch colours. 
   - Expected result: Players can only tell colours of  balls selected in central area, and are unable to tell colours of balls selected in edge area.

2. *Second Scene: Peripheral Light Detection*
   - Description: In this scene, a small area will be selected randomly to light up each time. It can be at the centre of the sphere, or at the edge of the sphere, which require player to detect with peripheral vision.
   - Goal: To test if peripheral vision is more sensitive to light. 
   - Expected result: Players could easily detect light in edge area even better than the central area.

3. *Third Scene: Peripheral Motion Detection in Dark Environment*
   - Description: In this scene, a  ball will be selected randomly and jiggles each time. It can be at the centre of the sphere, or at the edge of the sphere, which require player to detect with peripheral vision.
   - Goal: To test if peripheral vision is sensitive to motion.
   - Expected result: Players could easily detect motion in edge area even better than the central area.

4. *Fourth Scene: Peripheral Motion Detection in Light Environment*
   - Description: In this scene, a  ball will be selected randomly and jiggles each time. It can be at the centre of the sphere, or at the edge of the sphere, which require player to detect with peripheral vision. The background is much brighter than scene 3.
   - Goal: To test if peripheral vision is less sensitive to motion in brighter environment than dark environment.
   - Expected result: Players find it harder to detect motion than scene 3.

### Exit to Main Menu
- Each experiment scene features an *Exit Button* that allows users to return to the main menu after completing their observation.

---

## Technology Stack

### Unity and XR Interaction Toolkit
- Built using Unity with the *XR Interaction Toolkit* to enable seamless VR interaction.

### Hardware
- Tested on *Meta Quest 2* for immersive VR functionality.

---

## How to Run

### Editor setup
1. *Setup*:
   - Clone this repository to your local machine.
   - Open the project in Unity (version 2021.3 or later recommended).

3. *Run the Application*:
   - Launch the application from your Meta Quest 2 headset.
   - Use the main menu to select and explore the experiments.
### Standalone build
+ Download the standalone build from _Releases_ 
+ Connect your headset
+ Run Peripheral vision experiment.exe
---

## Notes

- This project is for educational and experimental purposes, focusing on how different visual factors impact peripheral vision.
- Ensure your play area is clear and safe when using the Meta Quest 2.

---
## License
This project is open-source and available under the [MIT License](LICENSE).

---

## Acknowledgments
- Special thanks to the Unity and XR Interaction Toolkit communities for their resources and support.
- Tested on Meta Quest 2 for reliable VR performance.
- This project is designed by students from University of Oulu.
