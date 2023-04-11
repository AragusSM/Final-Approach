FINAL APPROACH VR DESIGN DOCUMENT
Purpose
This was our final project for our VR Development for Games class in the spring of 2023. Our team members consisted of Danny Bernal, Neal Davar, Austin Nguyen, Davie Nguyen, Michael Ning, Anand Valavalkar. 

Overview
The player is an air traffic controller responsible for managing an airport's airplane takeoffs and landings. The player is in a control tower with monitors and controls allowing them to view and control arriving and departing planes. The player must manage the plane's resources and runway timers without causing airplane collisions and prioritizing on time arrivals and departures. The player’s goal is to manage the airplanes' takeoffs and landings efficiently without collisions, and allow the highest number of ontime arrivals and departures. In addition, the fuel level of the planes will allow for higher points to be scored at the end. Planes landed or taken off can also be scored according to the priority of the flights.

Ideation Phase
We came up with our idea from listing themes we were individually interested in. Aviation came up as one of the interests and we ultimately voted on it as our theme which evolved into air traffic control.



We also considered the ideas of wave based survival games (wizards and magic, bloons tower defense) as well as matching/pattern games (objective matching manufacturing game, DJ simulator).

Playtest Feedback
Our playtest yielded some very good feedback, from both the TA and from fellow students. One of the main pieces of feedback concerned the clarity of the instructions and gameplay. Some of the playtesters were initially confused about how to play the game and also what the overall point of the game was, at least prior to one of the developer’s verbal guidance. We did also receive a lot of positive feedback; playtesters were impressed with the graphics and being able to see their actions translate to actual planes taking off and arriving. With this game being somewhat of a puzzle-type/strategy game, playtesters said they were excited to test their brain power. 

Changes that we made in response to this feedback, was to add an instruction screen/page that players can read before clicking start.This should be a comprehensive tutorial video that plays before starting and will have coinciding, clear text.

Narrative Design
There is not much of a story, but more of a simulation. Let’s say that an individual would like to one day become an air traffic controller; our game could function as a method for that individual to simulate what it's like to manage planes in a virtual scenario whilst under a similar, albeit on a completely different scale, stressful environment. 

Since there is not really a storyline per se, our game is much more about discovery and animations. The user’s goal is to manage planes, based on given information (ie. fuel levels, time of arrivals, time of departures), to safely reach their destinations with punctuality. Animations are apparent when users signal for takeoff or arrival, which enhance the user’s experience as an air traffic controller.

Gameplay and Mechanics
Once the user selects a level and clicks the starting button, a series of flights will appear on the dashboard monitor, each with certain stats. Planes must be optimally assigned to runways in order to maximize the points earned. Planes that pass a waiting time will earn no points while a plane that loses its fuel while in the air instantly causes the player to lose the game. The goal is to get as many points to advance to the next level, where the game will become more complex with the addition of runways and airplane types.

Our game includes a few mechanics that are quite apparent as a user attempts to achieve victory and get the highest score. Users can click/push buttons on the console, swipe through a menu of planes, grab door handles to enter and exit the tower and teleport (via ray-induced click) throughout the environment. Also included are additional scenes that allow the user to navigate through the airport using a vehicle such as a car or firetruck.




Aesthetic Style
Our Aesthetics mostly feature a modern - futuristic Air traffic control scene. The actual gameplay UI is a sort of hologram panel projection where the user can interact using ray cast.The Air traffic control center features an area with many monitors and computers, representative of what a real ATC would look like (sort of).



Our Airport is modeled after a real airport in Germany, which is where we built most of our environment from. We then obtained our planes from a realistic planes asset pack. Since our game is designed to be more of a simulation, our goal was to make the process seem as realistic as possible while still having feasible and intuitive player interactions. 


Technical Documentation
Plugins
XR Interaction Toolkit 

Asset Packs
Airport Scene - https://assetstore.unity.com/packages/3d/environments/high-detail-airport-mobile-234052?aid=1011l37TN&pubref=2020&utm_campaign=unity_affiliate&utm_medium=affiliate&utm_source=partnerize-linkmaker

Interior Design - https://assetstore.unity.com/packages/3d/environments/sci-fi/free-lowpoly-scifi-110070

Skybox - https://assetstore.unity.com/packages/2d/textures-materials/sky/allsky-free-10-sky-skybox-set-146014


Code Files
GameManager.cs: keeps track of and updates game states and scores
StartButton.cs: small script that updates game state
ScoreKeepingScript.cs: small script that controls when to display score
ATC.cs: bulk of gameplay logic (selecting airplanes, runways)

The ATC class is the overarching class that manages a lot of gameplay. It contains the game objects which represent our terminal and score board. Likewise, it manages the interactions the player has with the game objects and displays things like score and other relevant information for the user to know the status of the planes they are facilitating the take off and landing for.

Airplane.cs: keeps track of airplane data, states, and scoring

The Airplane class is used to create airplane objects and has important properties like the departure to signify whether the plane is a departing plane or arriving plane, waitingTime, passengersOnBoard and more. In the Update method of the Airplane, its properties and overall status are updated. For starters, the fuel level is decremented depending on the plane size. Then, if the plane is taking off, we update the user’s score depending on factors such as how long the plane had to wait and the plane class. On the other hand if the plane is landing and on its way back to the terminal, we once again update the user’s points based on factors such as the plane class and the fuel level of the plane. 

Terminal.cs: spawns airplanes and maintains data structure

The Terminal class manages planes that are at the terminal and has methods like createPlane and createButton that are linked to the terminal instance in the ATC. Over time, as the user lands and takes off more planes, more planes will be populated in the terminal list as a result of the aforementioned methods called in the Update function of the Terminal class.

Sky.cs: spawns airplanes and maintains data structure

The Sky class mimics the structure and has many of the same properties as the Terminal class with the only difference being the planes being created have different properties. For instance, these planes will have a sky property but will have no terminal property. 

DepartureButton.cs: controls what and how data is displayed on buttons. 

The departure button is the class that controls the button on the arrivals and departures monitor in-game. As new flights are generated, these buttons will also generate and display the data of its corresponding airplane. It also controls aesthetic features of the button in-game such as a dynamically decreasing fuel icon.

ArrivalButton.cs: controls what and how data is displayed on buttons (similar in structure to the departure button)


RunwayButton.cs: controls what and how data is displayed on buttons

Runway button gameobject. This clears the current plane for takeoff or to return to the terminal after landing and frees its associated runway object. It also triggers the custom animation and audio of the airplane movement.

Runway.cs: The class for the runway object. 

The runway has its assigned plane object (one plane per runway) as well as 2 timers that control taxiing time and returning time. Finally, during Update, the runway can change it status to open or closed depending on the existence of a plane.

Code Structure & Conventions
The structure of our code revolved around giving each game object its own class and every interaction between game objects or the player has its own method.

Each class (Airplane, runway, departure button, etc. Has their own field as well as variables that link the game objects together (such as the ATC having a current airplane and runway selected). We attach these classes/scripts to the actual objects in unity.

Each class also has update functions that alter the fields on the game object. In addition, some classes have multiple methods that modify their objects (for example ATC modifies the runway and planes to match each other and alters aesthetic components in the scene).

All the objects, fields, and methods are combined together intricately under the game timer and serve to function as a cohesive game loop for the final, realistic experience
