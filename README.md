# Vending-Machine-Demo
A demo of vending machine logic. Based on .Net Core, Vue and SignalR. UI is very limited/absent. Bootstrap CSS is used for some basics

Open the solution file (VendingMachine.sln) with Visual Studio 2017 or higher. 
Hit F5 Debug to run.

A demo is available at https://vendingmachinedemo.azurewebsites.net

Some notes:

All the Vue code is in site.js located in the wwwwroot/js folder of the Vending machine web application project
All C# logic is in the Vending.Machine.Logic project.
Tests are in the Unit tests project.
A SignalR websocket connection is used for the connection of UI with the Server. Logic for that is in the Hub.cs file located in the VendingMachine project root.

Not added:
Security for clients via SignalR is not implemented. 
No Database or storage used. All runs in memory. To reset to default values just hit the red button or refresh teh page.
A better ui and mobile friedly. 
UI Logic does not include stock checks now.
Reconnection logic for web sockets in case of failure. So having a long session might break the connection and a refresh of the window is needed.
Tighter integration with node and vue based on parcel js, vuex and vue templates

