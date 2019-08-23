<%(TOC)%>

# Remote profiling

<%(Callout message="
This page discusses how to connect the [Unity Profiler](https://docs.unity3d.com/Manual/Profiler.html) to a worker running in a cloud deployment. 

Before reading this page, make sure you are familiar with:

  * [Build configuration]({{urlRoot}}/modules/build-system/build-config)
  * [Deploying]({{urlRoot}}/modules/deployment-launcher/overview)
")%>

The [Unity Profiler](https://docs.unity3d.com/Manual/Profiler.html) offers the ability to connect to an instance of your game running on a different machine. You can utilize this to profile a Unity worker running in a cloud deployment.

## 1. Configure your workers

The Unity Profiler connects to your worker via a host and port combination. The worker will only expose this port, if you enable the `Development Build` option for this worker. You can configure this via the [build configuration]({{urlRoot}}/modules/build-system/build-config) asset.

1. Find and open the instance of the [build configuration]({{urlRoot}}/modules/build-system/build-config) asset in the Unity Editor Inspector.
2. Expand the section for the worker type that you want to profile.
3. Expand the `Cloud Build Options` section within that worker type.
4. Click on the `Linux` tab to configure the `Linux` build options.
5. Ensure that the `Development` box is ticked.

<%(#Expandable title="What should it look like when you're done?")%>

![]({{assetRoot}}assets/workflows/remote-profiling-build-config.png)

<%(/Expandable)%>

## 2. Build the workers

Now that you configured your worker correctly to allow remote profiling, you can build your workers.

1. Select **SpatialOS** > **Build for cloud** > **All workers** from the Unity Editor menu to build all workers.

> Alternatively, if you don't need to rebuild all the workers, you can only build the worker type for which you enabled the `Development` option.

## 3. Upload worker assemblies & launch deployment

Use the [Deployment Launcher feature module]({{urlRoot}}/modules/deployment-launcher/overview) to upload the built out worker assemblies and launch a deployment.

## 4. Find the profiling port

At this point, your deployment should be up and running and your workers connected to that deployment.

Since your workers are running in the cloud, you need to forward the port that is exposed by those workers to your local machine in order to connect the profiler to it. Each worker should log a single message to SpatialOS detailing which port it is currently exposing.

1. Open the deployment in the SpatialOS console.
2. Navigate to the logs for your deployment.
3. Look for a message similar to the following: `[UnityGameLogic0:UnityGameLogic0] Unity PlayerConnection port: 55203.`
4. Note down both the worker ID (`UnityGameLogic0` in the example above) and the port (`55203` in the example above).

> **Note:** Since each worker will log their port, you will have more than 1 of these if you have more than one worker running! Select the worker that you are most interested in.

## 5. Forward the profiling port

You can forward the profiling port in two different ways, either through a Unity Editor window or using the `spatial` CLI directly.

<%(#Expandable title="Using the Unity Editor")%>

Open the Port Forwarding window by selecting **SpatialOS** > **Port Forwarding** in the Unity Editor menu.

Fill in the following information in the Port Forwarding window:

* `Deployment Name` is the name of the deployment that your worker is running in
* `Worker ID` is the ID for the worker which you wish to profile. (`UnityGameLogic0` in the example above)
* `Port` is the port that this worker is exposing. (`55203` in the example above)

It should look similar to the following:

![]({{assetRoot}}assets/workflows/remote-profiling-window.png)

> **Note:** The window will do some validation on the input and let you know if something looks a little off.

Select the `Forward Port` button to start the port forwarding in the background.

Its done when the window looks something like:

![]({{assetRoot}}assets/workflows/remote-profiling-window-success.png)

> **Note:** You must leave this window open until you are done profiling your worker. Closing the window will stop the port forwarding operation in the background.

<%(/Expandable)%>

<%(#Expandable title="Using the spatial CLI")%>

Run the following command in your terminal:

```bash
spatial project deployment worker port-forward -d ${DEPLOYMENT_NAME} -w ${WORKER_ID} -p ${PORT}
```

where:

* `${DEPLOYMENT_NAME}` is the name of the deployment that your worker is running in.
* `${WORKER_ID}` is the ID for the worker which you wish to profile. (`UnityGameLogic0` in the example above)
* `${PORT}` is the port that this worker is exposing. (`55203` in the example above)

Its done when you see a message similar to the following in your console:

```text
Established tunnel with worker instance UnityGameLogic0:55203, listening locally on 55203
```

> **Note:** You should leave this terminal open until you are done profiling your worker. If you close the terminal or kill the `spatial` process, the port forwarding will terminate and you will be unable to connect the profiler to that worker.

<%(/Expandable)%>

## 6. Connect the Unity Profiler to your worker

Now all that's left to do is to connect the Unity Profiler to your worker!

1. Select **Window** > **Analysis** > **Profiler** to open the Unity Profiler window.
2. Select the `Editor` button on the toolbar in the Unity Profiler window.
3. Select the `<Enter IP>` option.
4. Enter `127.0.0.1` to the box that pops up.
5. Select `Connect` on the popup.

The Unity Profiler is now connected to the worker that is running in the cloud deployment!
