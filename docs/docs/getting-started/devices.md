# Devices

Once you have a [Server](./server) running, telemetry sent by devices can be
received by the Agrigate EventService on whichever MQTT topic was specified in
the server's `docker-compose.yml`

The device payload should resemble the following:

```
{
    "DeviceId": 0,
    "Key": "SensorName",
    "Value": 0.24,
    "Unit": "F",                                # Optional
    "Timestamp": "2024-09-30T00:00:00.000Z"     # Optional
}
```

:::warning

In order for the telemetry to be successfully saved to the database, there must
be a device with the specified DeviceId in the database already. You'll receive
the device's DeviceId when making a POST request to
`http://localhost:8081/devices`

:::
