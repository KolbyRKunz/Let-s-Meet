//@ts-check

import 'react-native-gesture-handler';
import React from "react";
import { Text, StatusBar } from 'react-native';
import { NavigationContainer } from "@react-navigation/native";
import { createStackNavigator } from '@react-navigation/stack';
import HttpExample from './http_example.js'
import Demo from './demo.js'
import Navigator from './routes/drawer';


export default function App () {
  return(
    <>
      <Text>
        Hello World;
      </Text>
      <Navigator />
    </>
  );
};




//Sources for trying to get drawer navigator to work
//https://www.youtube.com/watch?v=EaNCi8o8H0A&ab_channel=TheNetNinja
//https://github.com/iamshaunjp/react-native-tutorial/tree/lesson-24
//https://reactnavigation.org/docs/drawer-navigator




