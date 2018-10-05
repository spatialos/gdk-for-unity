# Fake players in the cloud

// Intro - what's covered, what's in it for the reader. Encourage them that it'll be great. Tell them

### What are fake clients?

// Concise description - what are they and why do we want them.

### High level steps

* Build all workers
* Upload worker assemblies
* Launch a cloud deployment
* Join the game using the launcher
* Scale up the fake clients with worker flags!
* Use the cloud inspector and metrics panel to observe the game state
* Dynamically tweak fake client numbers
* Share the deployment link with friends

### Building workers (3-4 minutes)

While you are developing with the SpatialOS GDK for Unity the code for both of your workers can execute from within your editor. This allows for very rapid iteration, so your code changes incrementally take effect immediately without the need for a full re-build.

For a cloud deployment you will build out the workers so they can be uploaded to the cloud.

In the Unity editor you can initiate the building of your workers from the SpatialOS menu by clicking **Build all workers for cloud**.

### Uploading (connection dependent)

#### Setting your project name (first time only)

Your SpatialOS account on the website is associated with a generated SpatialOS "project name" that you'll need to use when uploading your project. You can log on to the SpatialOS web console to find this project name, but it will also appear in an email you received after signup.

https://console.improbable.io/projects

The project name is generated as string of random words. Copy it, for use in just a moment.

Using a text editor of your choice, open the `spatialos.json` file found in the root directory of your SpatialOS project. If you are using a SpatialOS starter project then the project name will be `unity_gdk` or `your_project_name_here`. Replace this name with your randomly generated project name given to you by SpatialOS.

This will let the SpatialOS platform know which project you intend to upload to.

#### Upload worker assemblies

An assembly is a bundle of code, art assets and other files necessary to run your game in the cloud.

// Tell them about terminals...
// spatial upload

#### Launch a cloud deployment

// Again, remind them they'll need to do it in a terminal...
// spatial cloud launch (get exact command and parameters from Matt. use large config?)
// Tell them how to use the console to check the deployment's status?

#### Get yourself in-game

// Steer them to the console https://console.improbable.io/
// Tell them about the launcher
// Get them to launch into the game
// Encourage to run up to a good vantage point (so they can't be easily shot, and can see the fake players)

#### Starting up fake clients

// Tell them a little bit about the fake client setup: coordinators, with a "fake_client_count" which is PER coordinator
// Tell them briefly about worker flags
// Explain how to update worker flag values in the console
// Explain that the interval value allows fake client connections to be staggered to ease the load and better mimic players connecting
// Encourage them to set the fake_client_count from 0 to something like 8 (note how many UnityGameLogic workers there are, as there will be a coordinator per worker and therefore fake_client_count * numOfWorkers = total fake clients)

#### Observe your deployment

// Get hyped about the tooling
// Open the metrics panel from the console to see graphs of entities joining the world. Point out you can see the interval staggering of client connections.
// Open the inspector and see markers for fake clients moving around. Encourage them to select tick boxes for some logic workers for the fake clients to see the parts of the world those workers have in view.
// Point out load metrics in the

#### Dynamically tweak fake client numbers

// Worker flags can be updated again!
// Encourage them to set the number high (give a suggested number that will equal about 200 fake clients i.e. 200/number of coordinators)
// Mention load numbers in inspector will be under some strain - perhaps highlight that these fake clients are representative of 200 players connecting into the same FPS game!
// Encourage them to set the fake player count to zero and watch in the inspector as the coordinators disconnect the fake clients

#### Share with friends

// Briefly explain where in the console they can get shareable links so that their friends can jump into the same deployment.
// Encourage them to play and explore. Mention that if they want to make changes to the art, logic etc they will need to re-upload a new assembly.

#### Killing the world

// Teach them how to stop a running deployment. And mention how they'd go about starting it back up.
