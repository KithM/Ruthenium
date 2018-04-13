![alt text](https://i.imgur.com/673fMPp.png "Ruthenium Logo")

## Summary
Ruthenium is a free open-source computer simulation game, which allows users to operate and configure a virtual system to their will, and features a collective database of users, of which can be managed and modified, hold [permissions](https://github.com/KithM/Ruthenium/wiki/Permissions), and more, all of which are modifiable inside and outside of the game, and allows the player to customize their gameplay as they want.

## Installation
<b>[Download the latest release of Ruthenium](https://github.com/KithM/Ruthenium/releases/latest)</b><br>

## Play
Launch the game with `ruthenium.exe`, select your desired resolution, and you'll be presented with a log-in screen. Two users already exist after launching the game so far: `default` and `admin`. Both of the users will have the password of `password` and will be able to be logged into right away. `default` is your basic, permissionless user, whereas `admin` is your system moderator, given permissions to be able to assign permissions to themselves and other users.

![alt text](https://i.imgur.com/XeabokP.png "Log-in Screen")

Log-in to the `default` or `admin` user and you will be welcomed with a desktop, complete with a system-based time, a taskbar, and a few desktop icons. 

<b>Click the Profile icon</b>. This is your current user. You will see your `Bio`, `UserGroup`, and profile picture. Want to quickly change your bio? <b>Click the edit button</b> and start typing away.

![alt text](https://i.imgur.com/5voeO68.png "User Profile")

<b>Click the Users icon</b>. This is the user database. It contains the list of all of the users that your current user is able to see. If you do not see your user (indicated by being marked as bold), your user is hidden, and your user does not contain the permission `settings.see.hidden`. This will also allow you to see other users that contain the permission node `user.type.hidden`.

![alt text](https://i.imgur.com/BwWiHZO.png "User Database")

<b>Click the Permission Manager icon</b>. You will notice if you logged into `default` you don't have any permissions. If you've logged into `admin`, you will have four permissions. You can view other users' permissions to make sure everything looks good by clicking the <b>user dropdown list</b>, and selecting a user. The permissions will update according to what user is selected. 

If your current user has the permission node `permissions.add.self` and/or `permissions.remove.self`, you will be able to change your own permissions. You can remove permissions by clicking the red <i>X</i> icon, and you can add permissions by typing in the permission node (case sensitive) into the large input box and hitting <b>Add Permission</b>.

If your current user has the permission node `permissions.add.others` and/or `permissions.remove.others`, you will also be able to change other users' permissions.

![alt text](https://i.imgur.com/pQXXJoK.png "Permission Manager")

<b>Click the Manage icon</b>. You will see a box that displays your current `Username`, `Password`, and `Session ID`. You will also notice that your password has a colored bar underneath of it. The bar represents your password's strength is regards to its length and/or what characters are used. Passwords that involve special characters and uncommon symbols are always much stronger than simple passwords.

If your current user has the permission node `settings.manage.self`, you will be able to edit your `Username` and `Password` in the input boxes that display them. Keep in mind that you will not be able to have an empty `Username` or `Password`. If you want to <i>delete your current user</i>, you can <b>click the delete button</b> and you will be logged out of the account and it will be removed from the database.

![alt text](https://i.imgur.com/Q9yDreT.png "User Management")

<b>Click the Reload icon</b>. This is useful for updating the database on-the-fly, such as if you want to keep the game open when making changes to the `data.ruth` file or other files.

## Contribute
- <b>[Submit a bug or problem in the current build](https://github.com/KithM/Ruthenium/issues/new)</b>
- <b>[Make a suggestion for a future build](https://github.com/KithM/Ruthenium/issues/new)</b>
