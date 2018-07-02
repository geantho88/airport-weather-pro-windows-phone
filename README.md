# airport-weather-pro-windows-phone
High quality real-time information about the weather conditions in all major airports in Greece.

This application contains high quality real-time information about the weather conditions in
all major airports in Greece.
The user can navigate to any airport throw the list and get informed about the weather
conditions like :

 Sky conditions
 Temperature
 Moisture
 Visibility
 Wind
 Air pressure

Data update is performed automatically by navigating through the list at each airport
separately and the values are stored on user’s device (using sqlite) until the next use of the
application in order to be available if there is no connection of the device to the Internet.
This application is designed using the MVVM model and data binding.

Model - View Model – View are three separated concepts and the data is stores in Sqlite ,
xml files and isolated settings files according to the specification of each operation of the
application

The data provider is an Xml Soap Web service thanks to http://www.webservicex.net
