# Unity_AssetCreationHelper

``AssetCreationHelper.cs`` provides some functions to create asset in your ``Unity/Assets`` dir.

## Import to Your Project

You can import this asset from UnityPackage.

- [AssetCreationHelper.unitypackage](https://github.com/XJINE/Unity_AssetCreationHelper/blob/master/AssetCreationHelper.unitypackage)

## How to Use

There is no sample in this repository. If you need any sample, please check some another asset.


| Function                             | Abstract   |
| ------------------------------------ | ---------- |
| CreateAssetInCurrentDirectory        | Create asset in current ``Project/Assets`` dir.                    |
| GetCurrentAssetDirectoryPathAbsolute | Get current directory absolute path in ``Project/Assets`` dir.     | 
| GetCurrentAssetDirectoryPathRelative | Get current directory relative path in ``Project/Assets`` dir.     |
| StartToRenameAsset                   | Start to rename specified asset.                                   |
| FocusAsset                           | Focus specified asset without any animation like a ``PingObject``. |

``FocusAsset`` is quite difficult to realize with default Unity library. This function use reflection.