const pak = require('./package.json');

module.exports = {
  presets: ['module:metro-react-native-babel-preset'],
  plugins: [
    'react-native-reanimated/plugin'
  ]
};
//^^^https://github.com/software-mansion/react-native-reanimated/issues/1875#issuecomment-816554755
//Put in while figuring out draw navigator issue
//https://lifesaver.codes/answer/unable-to-resolve-module-node-modules-react-native-packager-184
//^^actually this one plus the command it says: npm tare -- --reset-cache
//The github one kept throwing crazy errors because there was extra unnecessary stuff in the guy's code
//that we don't have/need.


// module.exports = {
//   presets: ['module:metro-react-native-babel-preset'],
// };
//^^what was here before
