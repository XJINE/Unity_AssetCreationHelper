# Unity_AssetCreationHelper

``AssetCreationHelper.cs`` provides some functions to create asset in your ``Unity/Assets`` dir.

## Importing

You can use Package Manager or import it directly.

```
https://github.com/XJINE/Unity_AssetCreationHelper.git?path=Assets/Packages/AssetCreationHelper
```

## How to Use

There is no sample in this repository. If you need any sample, please check some another asset.

- https://github.com/XJINE/Unity_MenuItemCreateText

| Function                             | About                                                               |
| ------------------------------------ | ------------------------------------------------------------------- |
| CreateAssetInCurrentDirectory        | Create asset in current ``Project/Assets`` dir.                     |
| GetCurrentAssetDirectoryPathAbsolute | Get current directory absolute path in ``Project/Assets`` dir.      | 
| GetCurrentAssetDirectoryPathRelative | Get current directory relative path in ``Project/Assets`` dir.      |
| CorrectAssetNameToAvoidOverwrite     | Rename asset with suffix number if it is needed to avoid overwrite. |
| StartToRenameAsset                   | Start to rename specified asset.                                    |
| FocusAsset                           | Focus specified asset without any animation like a ``PingObject``.  |

``FocusAsset`` is quite difficult to realize with default Unity library. This function use reflection.
