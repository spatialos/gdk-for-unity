# 6. Check out what’s happening in your game world

Let’s take a look at how many simulated player-clients are now running around this world and how our deployment is performing, using the World Inspector accessible from the Deployment Overview page:

<img src="{{assetRoot}}assets/overview-page-inspector.png" style="margin: 0 auto; display: block;" />

The [World Inspector](https://docs.improbable.io/reference/latest/shared/operate/inspector#inspector) provides a real time view of what’s happening in a deployment, from the [perspective of SpatialOS](https://docs.improbable.io/reference/latest/shared/concepts/spatialos): where all the entities are, what their components are, which workers ) provides a real-time view of what’s happening in a deployment, from the [perspective of SpatialOS](https://docs.improbable.io/reference/latest/shared/concepts/spatialos): where all the entities are, what their components are, which workers are running and which entities they are reading from and writing to.

We can use it, for instance, to highlight where all the simulated player-clients and player-entities are in the world (note: not cool to identify where your friends are hiding).

<img src="{{assetRoot}}assets/inspector-simulated-player.png" style="margin: 0 auto; display: block;" />

[The Logs app](https://docs.improbable.io/reference/latest/shared/operate/logs#logs), available one tab away, displays all your deployment’s logs (whether they come from the SpatialOS Runtime or the Worker code you have written) with useful filters.

<img src="{{assetRoot}}assets/logs-app.png" style="margin: 0 auto; display: block;" />

The [Metrics dashboards](https://docs.improbable.io/reference/latest/shared/operate/metrics#metrics), one more tab to the right, show a selection of useful metrics, with annotations to identify the health of your deployment.

<img src="{{assetRoot}}assets/metrics.png" style="margin: 0 auto; display: block;" />

<%(Callout type="info" message="Note that the Logs and Metrics are [available via an API](https://docs.improbable.io/reference/latest/shared/operate/byo-metrics) if you wish to create your own dashboards or alerts.")%>

## Game running nicely! Want to add more features?

You can add your first feature by following the [Health Pick-up tutorial]({{urlRoot}}/projects/fps/tutorial).


**[[1]]({{urlRoot}}/content/get-started/get-started1.md) [[2]]({{urlRoot}}/content/get-started/get-started2.md) [[3]]({{urlRoot}}/content/get-started/get-started3.md) [[4]]({{urlRoot}}/content/get-started/get-started4.md) [[5]]({{urlRoot}}/content/get-started/get-started5.md) < Back**

