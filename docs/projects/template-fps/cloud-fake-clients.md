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

To run a deployment in the cloud, you must upload the worker assemblies to your SpatialOS project. This can only be done through the spatial CLI. You must also give the worker assemblies a name so that you can reference them when launching a deployment.

Using a terminal of your choice, navigate to the root directory of your SpatialOS project and execute `spatial cloud upload <assembly_name>` where `assembly_name` is your chosen name.

> **It’s finished uploading when:** You see `spatial upload <assembly_name> succeeded` printed in your terminal output.

#### Launch a cloud deployment

The next step is to launch a cloud deployment using the worker assembly that you just upload. This can only be done through the spatial CLI.

When launching a cloud deployment you must provide three things:

* the assembly name, which identifies the worker assemblies to use
* a launch configuration, which declares the world and load balancing configuration
* a name for your deployment, which is used to label the deployment in the SpatialOS console

Using a terminal of your choice, navigate to the root directory of your SpatialOS project and execute `spatial cloud launch <assembly_name> cloud_launch_large.json <deployment_name>` where `assembly_name` is the name you gave the assembly in the previous step and `deployment_name` is a name of your choice.

> **It's finished when:** TODO - see what this says.

#### Get yourself in-game

Once your cloud deployment has started, you can launch a client from the [SpatialOS console](https://console.improbable.io/projects).

Using a web browser, navigate to [https://console.improbable.io/projects](https://console.improbable.io/projects) and find your deployment under your project. Click on the name of your deployment to be taken to the deployment overview page.

From this page, you can manage your deployment and launch clients via the [SpatialOS Launcher](fix). On the left hand side of the web page, press Launch and then press Launch again on the popup. This will start the SpatialOS Launcher which will download the game client assembly and start it.

Once the launcher has started the game client, you will be prompted to select the resolution and quality of the game. When you are happy with the settings, press Play! to start the game.

To control your character, use WASD to move and mouse to look around. Find a good vantage point and look around, you should see the fake clients running around and shooting each other.

#### Starting up fake clients

// Tell them a little bit about the fake client setup: coordinators, with a "fake_client_count" which is PER coordinator
// Tell them briefly about worker flags
// Explain how to update worker flag values in the console
// Explain that the interval value allows fake client connections to be staggered to ease the load and better mimic players connecting
// Encourage them to set the fake_client_count from 0 to something like 8 (note how many UnityGameLogic workers there are, as there will be a coordinator per worker and therefore fake_client_count * numOfWorkers = total fake clients)

#### Observe your deployment

// TODO: Split up this section?

Now lets explore the deployment overivew page and see what the SpatialOS console allows you to do.

Using a web browser, navigate to [https://console.improbable.io/projects](https://console.improbable.io/projects) and find your deployment under your project. Click on the name of your deployment to be taken to the deployment overview page.

On the main deployment overview page, on the right hand side you have a details pane. This contains a variety of information about your deployment. You should also see links to Logs and Metrics, the SpatialOS console provides a logging and metrics infrastructure out of the box.

Click on the Metrics link and then Public Default option to be taken to the Metrics page. This allows you to check the health and state of your deployment and can aid in diagnosing and debugging problems. On the top-right hand side, press Dashboards and then "Debug entities and command".

This dashboard shows you a graph of entity count as well as when entities were created. You can also see these entities visualised spatially in the Inspector, at the top of the page, click on World to be taken to the Inspector.

The Inspector gives you a real-time visualisation of your world, each small marker in the inspector view represents an entity, you can customise the colour and icon of these markers to make the world state easy to understand from a glance.

The Inspector also allows you to visualise what your worker's view is. On the top right hand side, select one or more of the tick boxes under the Workers tab. This will highlight areas in the Inspector corresponding to the write authority region for that work. 

Find the small box labeled "Show where workers can" in the upper right hand side of the Inspector and select the Read tickbox, this will show the Read access of each worker shown with a ticked border.

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
