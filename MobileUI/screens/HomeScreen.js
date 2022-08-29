import React, { useState } from 'react';
import { Button, Alert, Text, StatusBar, SafeAreaView, StyleSheet } from 'react-native';
// import HttpExample from './http_example.js'
// import Demo from './demo.js'
import { Calendar, CalendarList, Agenda } from 'react-native-calendars';
import { LocaleConfig } from 'react-native-calendars';
import Colors from '../assets/styles/colors';
import createEventPopup from './createEventPopup';
//import { Button, View } from 'react-native';


//TODO: In Calendar, can't get the selected day background to changecolor like advertised

//About: There are two versions we could go with: plain old calendar or the agenda version
//The agenda version has built in events and things that make it cool https://github.com/wix/react-native-calendars#agenda
//Or we do our own thing with plain calendar

LocaleConfig.locales['fr'] = {
  monthNames: [
    'January',
    'February',
    'March',
    'April',
    'May',
    'June',
    'July',
    'August',
    'September',
    'October',
    'November',
    'December'
  ],
  monthNamesShort: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'June', 'Jul', 'Aug', 'Sept', 'Oct', 'Nov', 'Dec'],
  dayNames: ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'],
  dayNamesShort: ['S', 'M', 'T', 'W', 'T', 'F', 'S'],
  today: "Today"
};
LocaleConfig.defaultLocale = 'fr';

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: Colors.DD_CREAM,
    justifyContent: 'space-evenly'
  },
  outputContainer: {
    color: Colors.DD_RED_2,
    left: 50,
    maxWidth: 300,
    padding: 15,
    borderWidth: 15,
    borderColor: Colors.DD_RED_2,
    backgroundColor: Colors.DD_CREAM,
    fontWeight: 'bold'
  },
  text: {
    fontSize: 25,
    fontWeight: '500',
    color: Colors.DD_RED_2,
    textAlign: 'center',
    //marginTop: 10,
    //marginBottom: 50,
    paddingHorizontal: 90,
    position: 'relative',
    flexWrap: 'wrap'
  }
});

const TempOutputBox = (props) => {
  return (
    <Text style={styles.outputContainer}>{props.day.dateString} was selected</Text>
  );
}

const CalendarTitle = (props) => {
  return (
    <Text style={styles.text}>{props.groupName} Schedule</Text>
  );
}

//Keep for posterity just in case we need another button to interact with navigator
// function HomeScreen({ navigation }) {
//   return (
//     <View style={{ flex: 1, alignItems: 'center', justifyContent: 'center' }}>
//       <Button
//         onPress={() => navigation.navigate('Notifications')}
//         title="Go to notifications"
//       />
//     </View>
//   );
// }



const initialMarkedDateList = []; //this is where we will plug in all the stuff from database

function HomeScreen(props) {
  const [selectedDay, setSelectedDay] = useState('');
  const [markedDateList, setMarkedDateList] = useState(initialMarkedDateList);
  const [mark, setMark] = useState({});

//TODO: none of this is gonna make sense until I relearn about life cycle and how states and hooks work.

  // let nextDate = [
  //   selectedDay.dateString,
  //   '2022-04-29',
  //   '2022-04-04',
  //   '2022-04-5',
  //   '2022-04-06',
  //   '2022-04-05',
  // ];

  //let mark = markDatesInList(markedDateList, selectedDay);
  //let mark = {};

  // nextDate.forEach(day => {
  //   //console.log(day);
  //   if (day == selectedDay.dateString) {
  //     mark[day] = {
  //       selected: true,
  //       selectedColor: Colors.DD_RED_3
  //     };
  //   } else {
  //     mark[day] = {
  //       marked: true,
  //       dotColor: Colors.DD_RED_3
  //     }
  //   }
  // });

  function handleAddToMarkedDateList() {
    const newMarkedDateList = markedDateList.concat(selectedDay.dateString);
    setMarkedDateList(newMarkedDateList);
    setMark(markDatesInList(markedDateList));
    console.log("(handleAdd)markedDateList: " + markedDateList);
    console.log("(handleAdd)mark: " + mark);
    // nextDate.push(selectedDay.dateString + '');
    // console.log(nextDate);
    // nextDate.forEach(day => {
    //   //console.log(day);
    //   if (day == selectedDay.dateString) {
    //     mark[day] = {
    //       selected: true,
    //       selectedColor: Colors.DD_RED_3
    //     };
    //   } else {
    //     mark[day] = {
    //       marked: true,
    //       dotColor: Colors.DD_RED_3
    //     }
    //   }
    // });
    Alert.alert("Event on " + selectedDay.dateString + " created");

  }

  function markDatesInList() {
    console.log("from function markDatesInlist: selectedDay = " + selectedDay.dateString);
    let mark = {};
    markedDateList.forEach(day => {
      console.log("from function markDatesInList (in foreach loop) day in given list " + day);
      if (day == selectedDay.dateString) {
        mark[day] = {
          selected: true,
          selectedColor: Colors.DD_RED_3
        };
      } else {
        mark[day] = {
          marked: true,
          dotColor: Colors.DD_RED_3
        }
      }
    });

    return mark;
  }

  return (
    <SafeAreaView style={styles.container}>
      <CalendarTitle groupName={props.groupName} />
      <Calendar
        onDayPress={day => {
          console.log('selected day', day);
          setSelectedDay(day);
          console.log("on day press, selectedDay = " + selectedDay.dateString);
          //console.log("set Mark and call function");
          //setMark(markDatesInList(markedDateList));
        }}

        // Handler which gets executed on day long press. Default = undefined
        onDayLongPress={day => {
          console.log('selected day', day);
        }}
        // markingType={'multi-dot'}
        // markedDates={{
        //   '2017-10-25': {dots: [vacation, massage, workout], selected: true, selectedColor: 'red'},
        //   '2017-10-26': {dots: [massage, workout], disabled: true}
        // }}
        // markedDates={{
        //   [selectedDay.dateString]: {
        //     selected: true,
        //     selectedColor: Colors.DD_RED_3
        //   }
        // }}

        //markedDates={mark}

        markedDates={markDatesInList()}

        // Month format in calendar title. Formatting values: http://arshaw.com/xdate/#Formatting
        monthFormat={'MMMM'}
        // Handler which gets executed when visible month changes in calendar. Default = undefined
        onMonthChange={month => {
          console.log('month changed', month);
        }}
        // Hide month navigation arrows. Default = false
        hideArrows={false}
        // Replace default arrows with custom ones (direction can be 'left' or 'right')
        //renderArrow={direction => <Arrow />}
        // Do not show days of other months in month page. Default = false
        hideExtraDays={true}
        // If hideArrows = false and hideExtraDays = false do not switch month when tapping on greyed out
        // day from another month that is visible in calendar page. Default = false
        disableMonthChange={true}
        // If firstDay=1 week starts from Monday. Note that dayNames and dayNamesShort should still start from Sunday
        firstDay={0}
        // Hide day names. Default = false
        hideDayNames={false}
        // Show week numbers to the left. Default = false
        showWeekNumbers={false}
        // Handler which gets executed when press arrow icon left. It receive a callback can go back month
        onPressArrowLeft={subtractMonth => subtractMonth()}
        // Handler which gets executed when press arrow icon right. It receive a callback can go next month
        onPressArrowRight={addMonth => addMonth()}
        // Enable the option to swipe between months. Default = false
        enableSwipeMonths={true}
        theme={{
          // backgroundColor: Colors.TEST_PURPLE, //dunno where this is supposed to show up
          calendarBackground: Colors.DD_CREAM,
          textSectionTitleColor: Colors.DD_CREAM,
          // textSectionTitleDisabledColor: '#d9e1e8',
          // selectedDayBackgroundColor: Colors.TEST_PURPLE,
          // selectedDayTextColor: Colors.TEST_PURPLE,
          todayTextColor: Colors.DD_LIGHT_GRAY, //meh suppper iffy on this one
          dayTextColor: Colors.DD_RED_2,
          // textDisabledColor: Colors.TEST_PURPLE,
          dotColor: Colors.TEST_PURPLE,
          //selectedDotColor: '#ffffff',
          arrowColor: Colors.DD_CREAM,
          // disabledArrowColor: '#d9e1e8',
          monthTextColor: Colors.DD_CREAM, //does it do anything?
          //indicatorColor: 'blue',
          // textDayFontFamily: 'monospace',
          // textMonthFontFamily: 'monospace',
          // textDayHeaderFontFamily: 'monospace',
          // textDayFontWeight: '300',
          // textMonthFontWeight: 'bold',
          // textDayHeaderFontWeight: '300',
          textDayFontSize: 20,
          textMonthFontSize: 40,
          textDayHeaderFontSize: 24
        }}
        style={{
          //borderWidth: 10, //def need to do fancy stuff to make that look good
          //borderColor: Colors.DD_GRAY,
          backgroundColor: Colors.DD_RED_2,
          paddingBottom: 10,
          //marginBottom: 50,
          //position: 'absolute',
          width: 390
        }}
      />
      {/* look below to see sources for this
       <Navigator /> */}
      <TempOutputBox day={selectedDay} />
      <Button
        title='Create Event'
        color={Colors.DD_RED_2}
        onPress={handleAddToMarkedDateList}
      />
    </SafeAreaView>
  );
};

HomeScreen.defaultProps = {
  groupName: "My"
}

export default HomeScreen;

//Sources for trying to get drawer navigator to work
//https://www.youtube.com/watch?v=EaNCi8o8H0A&ab_channel=TheNetNinja
//https://github.com/iamshaunjp/react-native-tutorial/tree/lesson-24
//https://reactnavigation.org/docs/drawer-navigator






//https://www.npmjs.com/package/react-native-calendars this is the calendar ap
// export default function App() {
//   return (
//     <SafeAreaView style={styles.container}>

//       <Agenda
//         // The list of items that have to be displayed in agenda. If you want to render item as empty date
//         // the value of date key has to be an empty array []. If there exists no value for date key it is
//         // considered that the date in question is not yet loaded
//         items={{
//           '2022-04-22': [{ name: 'item 1 - any js object' }],
//           '2022-04-23': [{ name: 'item 2 - any js object', height: 10 }],
//           '2022-04-24': [],
//           '2022-04-25': [{ name: 'item 3 - any js object' }, { name: 'any js object' }]
//         }}
//         // Callback that gets called when items for a certain month should be loaded (month became visible)
//         loadItemsForMonth={month => {
//           console.log('trigger items loading');
//         }}
//         // Callback that fires when the calendar is opened or closed
//         onCalendarToggled={calendarOpened => {
//           console.log(calendarOpened);
//         }}
//         // Callback that gets called on day press
//         onDayPress={day => {
//           console.log('day pressed');
//         }}
//         // Callback that gets called when day changes while scrolling agenda list
//         onDayChange={day => {
//           console.log('day changed');
//         }}
//         current={'2022-04-22'}
//         // Initially selected day
//         //selected={'2012-05-16'}
//         // Minimum date that can be selected, dates before minDate will be grayed out. Default = undefined
//         //minDate={'2012-05-10'}
//         // Maximum date that can be selected, dates after maxDate will be grayed out. Default = undefined
//         //maxDate={'2012-05-30'}
//         // Max amount of months allowed to scroll to the past. Default = 50
//         pastScrollRange={50}
//         // Max amount of months allowed to scroll to the future. Default = 50
//         futureScrollRange={50}
//         // Specify how each item should be rendered in agenda
//         renderItem={(item, firstItemInDay) => {
//           //return <View />;
//           return null;
//         }}
//         // Specify how each date should be rendered. day can be undefined if the item is not first in that day
//         renderDay={(day, item) => {
//           //return <View />;
//           return null;
//         }}
//         // Specify how empty date content with no items should be rendered
//         renderEmptyDate={() => {
//           //return <View />;
//           return null;
//         }}
//         // Specify how agenda knob should look like
//         renderKnob={() => {
//           //return <View />;
//           return null;
//         }}
//         // Specify what should be rendered instead of ActivityIndicator
//         renderEmptyData={() => {
//           //return <View />;
//           return null;
//         }}
//         // Specify your item comparison function for increased performance
//         rowHasChanged={(r1, r2) => {
//           return r1.text !== r2.text;
//         }}
//         // Hide knob button. Default = false
//         hideKnob={false}
//         // When `true` and `hideKnob` prop is `false`, the knob will always be visible and the user will be able to drag the knob up and close the calendar. Default = false
//         showClosingKnob={true}
//         // By default, agenda dates are marked if they have at least one item, but you can override this if needed
//         markedDates={{
//           '2022-04-16': { selected: true, marked: true },
//           '2022-4-17': { marked: true },
//           '2022-04-18': { disabled: true }
//         }}
//         // If disabledByDefault={true} dates flagged as not disabled will be enabled. Default = false
//         disabledByDefault={true}
//         // If provided, a standard RefreshControl will be added for "Pull to Refresh" functionality. Make sure to also set the refreshing prop correctly
//         onRefresh={() => console.log('refreshing...')}
//         // Set this true while waiting for new data from a refresh
//         refreshing={false}
//         // Add a custom RefreshControl component, used to provide pull-to-refresh functionality for the ScrollView
//         refreshControl={null}
//         // Agenda theme
//         theme={{
//            backgroundColor: Colors.DD_RED_2, //dunno where this is supposed to show up
//            calendarBackground: Colors.DD_RED_1,
//            textSectionTitleColor: Colors.DD_CREAM,
//           // textSectionTitleDisabledColor: '#d9e1e8',
//            selectedDayBackgroundColor: Colors.DD_RED_3,
//           // selectedDayTextColor: Colors.TEST_PURPLE,
//            todayTextColor: Colors.DD_LIGHT_GRAY, //meh suppper iffy on this one
//            dayTextColor: Colors.DD_RED_2,
//           // textDisabledColor: Colors.TEST_PURPLE,
//           // dotColor: Colors.TEST_PURPLE,
//            selectedDotColor: Colors.DD_CREAM,
//            arrowColor: Colors.DD_CREAM,
//           // disabledArrowColor: '#d9e1e8',
//            monthTextColor: Colors.DD_CREAM, //does it do anything?
//            indicatorColor: 'blue',
//           // textDayFontFamily: 'monospace',
//           // textMonthFontFamily: 'monospace',
//           // textDayHeaderFontFamily: 'monospace',
//           // textDayFontWeight: '300',
//           // textMonthFontWeight: 'bold',
//           // textDayHeaderFontWeight: '300',
//            textDayFontSize: 16,
//            textMonthFontSize: 16,
//            textDayHeaderFontSize: 16
//         }}
//         style={{
//           //borderWidth: 10, //def need to do fancy stuff to make that look good
//           //borderColor: Colors.DD_GRAY,
//           backgroundColor: Colors.DD_RED_2
//           //paddingBottom: 10
//           //marginTop: 100
//           //height: 350
//         }}
//       />

//       {/* look below to see sources for this
//       <Navigator /> */}
//     </SafeAreaView>
//   );
// };

//https://www.codegrepper.com/code-examples/javascript/react-native-calendar+selected+date
//https://blog.expo.dev/5-easy-to-use-react-native-calendar-libraries-e830a97d5bf7
//https://stackoverflow.com/questions/59598513/highlight-pressedselected-date-in-react-native-calendars

// export default function App() {
//   return (
//     <SafeAreaView style={styles.container}>
//       <Calendar
//         // Initially visible month. Default = now
//         //current={'2022-04-22'}
//         // Minimum date that can be selected, dates before minDate will be grayed out. Default = undefined
//         //minDate={'2012-05-10'}
//         // Maximum date that can be selected, dates after maxDate will be grayed out. Default = undefined
//         //maxDate={'2012-05-30'}
//         // Handler which gets executed on day press. Default = undefined
//         onDayPress={day => {
//           console.log('selected day', day);
//         }}

//         // markedDates={{ //Works but like, it's hard coded
//         //   '2022-04-16': {selected: true, marked: true, selectedColor: 'blue'},
//         //   '2022-04-17': {marked: true},
//         //   '2022-04-18': {marked: true, dotColor: 'red', activeOpacity: 0},
//         //   '2022-04-19': {disabled: true, disableTouchEvent: true}
//         // }}

//         markedDates={{[this.state.selected]: {selected: true, disableTouchEvent: true, selectedDotColor: 'orange'}}}

//         //markedDates={this.state.dateSelected} //https://github.com/wix/react-native-calendars/issues/766
//         //markedDates={{ [this.state.selected]: { selected: true } }} //https://github.com/wix/react-native-calendars/issues/905
//         // Handler which gets executed on day long press. Default = undefined
//         onDayLongPress={day => {
//           console.log('selected day', day);
//         }}
//         // Month format in calendar title. Formatting values: http://arshaw.com/xdate/#Formatting
//         monthFormat={'MMMM'}
//         // Handler which gets executed when visible month changes in calendar. Default = undefined
//         onMonthChange={month => {
//           console.log('month changed', month);
//         }}
//         // Hide month navigation arrows. Default = false
//         hideArrows={false}
//         // Replace default arrows with custom ones (direction can be 'left' or 'right')
//         //renderArrow={direction => <Arrow />}
//         // Do not show days of other months in month page. Default = false
//         hideExtraDays={true}
//         // If hideArrows = false and hideExtraDays = false do not switch month when tapping on greyed out
//         // day from another month that is visible in calendar page. Default = false
//         disableMonthChange={true}
//         // If firstDay=1 week starts from Monday. Note that dayNames and dayNamesShort should still start from Sunday
//         //firstDay={1}
//         // Hide day names. Default = false
//         hideDayNames={false}
//         // Show week numbers to the left. Default = false
//         showWeekNumbers={false}
//         // Handler which gets executed when press arrow icon left. It receive a callback can go back month
//         onPressArrowLeft={subtractMonth => subtractMonth()}
//         // Handler which gets executed when press arrow icon right. It receive a callback can go next month
//         onPressArrowRight={addMonth => addMonth()}
//         // Disable left arrow. Default = false
//         //disableArrowLeft={true}
//         // Disable right arrow. Default = false
//         //disableArrowRight={true}
//         // Disable all touch events for disabled days. can be override with disableTouchEvent in markedDates
//         //disableAllTouchEventsForDisabledDays={true}
//         // Replace default month and year title with custom one. the function receive a date as parameter
//         //renderHeader={date => {
//         /*Return JSX*/
//         //}}
//         // Enable the option to swipe between months. Default = false
//         enableSwipeMonths={true}
//       />
//       {/* look below to see sources for this
//       <Navigator /> */}
//     </SafeAreaView>
//   );
// };












