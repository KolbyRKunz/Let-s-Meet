import React, { useState } from 'react';
import { StyleSheet, View, Text, TouchableOpacity, FlatList } from 'react-native';
import { globalStyles } from '../styles/global';

export default function Home({ navigation }) {
  const [reviews, setReviews] = useState([
    
  ]);

  return (
    <View>
      <Text>Hello World!</Text>
    </View>
  );
}