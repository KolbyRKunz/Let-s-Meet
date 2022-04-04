//https://reactnative.dev/docs/network  function component version since I believe it's the most popular way
import React, { useEffect, useState } from 'react';
import { ActivityIndicator, FlatList, Text, View } from 'react-native';

export default HttpExample = () => {
  const [isLoading, setLoading] = useState(true);
  const [data, setData] = useState([]);
  const getMovies = async () => {
     try {
      console.log("try");
      const response = await fetch('http://192.168.56.1:3000/Api/Echo');
      console.log("response");
      const json = await response.json();
      console.log("this is the body " + json.body);
      setData(json.body);
    } catch (error) {
      console.log("catch");
      console.error(error);
    } finally {
      console.log("finally");
      setLoading(false);
    }
  }

  useEffect(() => {
    getMovies();
  }, []);

  return (
    <View style={{ flex: 1, padding: 24 }}>
      {isLoading ? <ActivityIndicator/> : (
        <Text>
            {data}
        </Text>
      )}
    </View>
  );
};


















// // https://www.tutorialspoint.com/react_native/react_native_http.htm
// import React, { Component } from 'react'
// import { View, Text } from 'react-native'

// class HttpExample extends Component {
//    state = {
//       data: ''
//    }
//    componentDidMount = () => {
//       fetch('http://10.0.2.2:37864/Api/Echo', {
//          method: 'GET',
//          headers:{
//             'Content-Type': 'application/json'
//          }
//       })
//       .then((response) => {   
//          response.json()
//          console.log(response)
//       })
//       .then((responseJson) => {
//          console.log(responseJson);
//          this.setState({
//             data: responseJson
//          })
//       })
//       .catch((error) => {
//          console.error(error);
//       });
//    }
//    render() {
//       return (
//          <View>
//             <Text>
//                {this.state.data}
//             </Text>
//          </View>
//       )
//    }
// }
// export default HttpExample