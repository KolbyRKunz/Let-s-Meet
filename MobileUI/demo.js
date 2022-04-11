import { StackView } from '@react-navigation/stack';
import React, { useEffect, useState } from 'react';
import { ActivityIndicator, FlatList, Text, View } from 'react-native';
import { isMessageIgnored } from 'react-native/Libraries/LogBox/Data/LogBoxData';

export default Demo = () => {
  const [isLoading, setLoading] = useState(true);
  const [dataU, setDataU] = useState([]);
  const [dataG, setDataG] = useState([]);
  const [dataE, setDataE] = useState([]);
  const getResponse = async () => {
     try {
      const responseUsers = await fetch('http://192.168.56.1:3000/Api/getUsers');
      const responseGroups = await fetch('http://192.168.56.1:3000/Api/getGroups');
      const responseEvents = await fetch('http://192.168.56.1:3000/Api/getEvents');
      const jsonU = await responseUsers.json();
      const jsonG = await responseGroups.json();
      const jsonE = await responseEvents.json();
      setDataU(jsonU);
      setDataG(jsonG);
      setDataE(jsonE);
    } catch (error) {
      console.error(error);
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    getResponse();
  }, []);


  return (
    <View style={{ flex: 1, padding: 24 }}>
      <Text>Users</Text>
      { isLoading ? <ActivityIndicator/> : 
        (dataU.map(item => <Text key={item.ID}>{item.FirstName} {item.LastName}</Text>))
      }
      <Text>{'\n'}</Text>

      <Text>Groups</Text>
      { isLoading ? <ActivityIndicator/> : 
        (dataG.map(item => <Text key={item.ID}>Group: {item.ID}</Text>))
      }
      <Text>{'\n'}</Text>

      <Text>Events</Text>
      { isLoading ? <ActivityIndicator/> : 
        (dataE.map(item => <Text key={item.ID}>Start: {item.startTime} End: {item.endTime} Group: {item.Group} {'\n'}</Text>))
      }
      <Text>{'\n'}</Text>
    </View>
  );
};

