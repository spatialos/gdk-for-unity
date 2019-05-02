<%(TOC)%>

# Get playing

Back in your SpatialOS [Console](https://console.improbable.io/projects), you should now see the two deployments that you just created appear under your project. Select the one without the `_sim_players` suffix to get to the Overview page:

<img src="{{assetRoot}}assets/overview-page.png" style="margin: 0 auto; width: 100%; display: block;" />

Select the **Launch** button on the left, and then select **Launch** (you can skip Step 1 - the SpatialOS Launcher was previously installed during setup). The SpatialOS Launcher will download the game client for this deployment and launch it.

<img src="{{assetRoot}}assets/launch.png" style="margin: 0 auto; display: block;" />

Once the client has launched, enter the game and fire a few celebratory shots - you are now playing in your first SpatialOS cloud deployment!

<img src="{{assetRoot}}assets/fps/by-yourself.png" style="margin: 0 auto; display: block;" />

It’s a bit lonely in there isn’t it? Keep your client running while we get this world populated.

## Invite friends

To invite other players to this game, head back to the Deployment Overview page in your SpatialOS Console, and select the **Share** button:

<img src="{{assetRoot}}assets/overview-page-share.png" style="margin: 0 auto; display: block;" />

This generates a short link to share with anyone who wants to join in for the duration of the deployment, providing them with Launcher download instructions and a button to join the deployment.

<img src="{{assetRoot}}assets/share-modal.png" style="margin: 0 auto; display: block;" />

## Invite enemies

**For more of a challenge, let’s now invite 200 enemies you can fight it out against!**

These enemies are simulated players; they are `UnityClient` client-worker instances running in the cloud, mimicking real players of your game from a behavior and load perspective. Their behavior is currently quite simple, but you could extend them to include additional gameplay features.

In fact, as far as SpatialOS is concerned, these simulated players are indistinguishable from real players; both simulated players and real players are just `UnityClient` client-worker instances to SpatialOS, so this is a good approach for regular scale testing.

The simulated players are hosted in a separate deployment to ensure that they do not share resources with your `UnityGameLogic` server-worker instances. You can find this deployment by returning to your SpatialOS [Console](https://console.improbable.io/projects) and selecting the deployment with the `_sim_players` suffix.

To get the legion of enemies started, you will use [Worker Flags]({{urlRoot}}/reference/glossary.md#worker-flags), which you can find from your SpatialOS Console's Deployment Overview page:

> **NOTE:** Make sure you have selected the Deployment **with** the `_sim_players` suffix.

<img src="{{assetRoot}}assets/overview-page-worker-flags.png" style="margin: 0 auto; display: block;" />

Modify the `fps_simulated_players_per_coordinator` flag value from 0 to 10 and hit save:

<img src="{{assetRoot}}assets/worker-flags-modification.png" style="margin: 0 auto; display: block;" />

What this will do is start up 10 simulated player-clients per Simulated Player Coordinator worker (of which there are 20 running in the deployment), and they will connect-in every 5 seconds (dictated by the `fps_simulated_players_creation_interval` flag).

<%(Callout type="warn" message="If you exceed 10 `fps_simulated_players_per_coordinator` you may experience deployment instability.")%>

Back in the game, you will soon see the new simulated player-clients running. Try to find them before they find you…

<img src="{{assetRoot}}assets/fps/enemies.png" style="margin: 0 auto; display: block;" />

## Job done!
Now you can take a look at your SpatialOS deployment to see what’s happening in your game world.

#### Next: [View your game world]({{urlRoot}}/projects/fps/get-started/view-game-world.md)
