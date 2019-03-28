<%(TOC)%>
# View your game world

Let’s take a look at how many simulated player-clients are now running around this world and how our deployment is performing, using the World Inspector accessible from the Deployment Overview page of the SpatialOS [web Console](https://console.improbable.io) (shown below).

<img src="{{assetRoot}}assets/overview-page-inspector.png" style="margin: 0 auto; display: block;" />
<br/>
<br/>
#### World Inspector
The [World Inspector](https://docs.improbable.io/reference/latest/shared/operate/inspector#inspector) (shown below) provides a real-time view of what’s happening in a deployment, from the [perspective of SpatialOS](https://docs.improbable.io/reference/latest/shared/concepts/spatialos): where all the entities are, what their components are, which workers are running and which entities a worker-instance is reading from and writing to.

We can use it, for instance, to highlight where all the simulated player-clients and player-entities are in the world (note: not cool to identify where your friends are hiding).

<img src="{{assetRoot}}assets/inspector-simulated-player.png" style="margin: 0 auto; display: block;" />
<br/>
<br/>
#### Logs
[The Logs tab](https://docs.improbable.io/reference/latest/shared/operate/logs#logs) (shown below), displays all your deployment’s logs (whether they come from the SpatialOS Runtime or the Worker code you have written).

<img src="{{assetRoot}}assets/logs-app.png" style="margin: 0 auto; display: block;" />
<br/>
<br/>
#### Metrics dashboards
The [Metrics dashboards](https://docs.improbable.io/reference/latest/shared/operate/metrics#metrics) (shown below), show a selection of useful metrics, to help you check health of your deployment and debug issues.

<img src="{{assetRoot}}assets/metrics.png" style="margin: 0 auto; display: block;" />
<br/>
<%(Callout message="Note that the Logs and Metrics are also [available via an API](https://docs.improbable.io/reference/latest/shared/operate/byo-metrics) if you wish to create your own dashboards or alerts.")%>

## Want to add more features?

This concludes our Getting Started guide - congratulations for taking your first steps with the GDK for Unity!

You can continue with the FPS Starter Project by following our [Health Pick-up tutorial]({{urlRoot}}/projects/fps/tutorial). This will teach you the basics of adding SpatialOS components and building a networked game feature in SpatialOS.
