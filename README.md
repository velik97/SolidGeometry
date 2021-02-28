# SolidGeometry
Mobile application for discovering solid geometry in AR.

Was made as part of a master's degree at MIPT.


Application has to important parts: editor and visulizer
In editor you can create new lessons, create figures at different steps, add descriptions and highlight parts
In visualizer you can view the result of your figures

## How to use editor
1) Open Scenes/LessonCreation/ConstructorScene.scene
2) Make sure "Scene" window is opened and is focused somewhere around (0,0,0)
3) In Unity top bar open Tools>Lessons Editor
4) In new window "Create New"
5) In "Shapes Set" tab you can create new shapes using "Create new Blueprint" button (Cube for instance)
6) Some shapes need others to exist. For instance PointOnLine can't exist wihtout line.
7) In "Stages Set" tab you can create stages of lesson. At each stage you can activate, deactivate or highlight some shapes
8) Don't forget to save lesson with "Save" button at the top of "Lesson Editor" window
