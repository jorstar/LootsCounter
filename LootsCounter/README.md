# Loots Counter

A counter bot for those who want to keep track of the amount of loots they get from https://loots.com/
You can have a maximum amount of loots in case you want to do anything with it, like after X amount you want to do something.
This is the uncompiled code so if you want to use it make sure to compile it with visual studio.

## Getting Started

The first time you open the program it will create a config file for you.
make sure to edit this config file before you continue.

## Config

<?xml version="1.0"?>
<Settings xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
   ```
   The username of the bot you want to use
   ```
  <BotUser>Username of the bot</BotUser>
  ```
  The twitch oauth token for the bot.
  ```
  <BotOauth>Oauth of the bot</BotOauth>
  ```
  The channel the bot has to join
  ```
  <ChannelName>Channel name to join</ChannelName>
  ```
  The username of the bot that is connected to your loots.com
  ```
  <LootsBotUser>Username of bot that sends loots messages</LootsBotUser>
  ```
  The link that the loots bot will post when you use !loots
  ```
  <LootsLink>Your own loots link</LootsLink>
  ```
  True or false, this option is if you want to reset the count.
  ```
  <ResetCounter>true</ResetCounter>
  ```
  Set the number at when you want to reset the count.
  ```
  <ResetAtCount>25</ResetAtCount>
  ```
  The message that will be send into chat when the counter gets reset
  ```
  <ResetMessage>Loots counter has been reset!</ResetMessage>
  ```
  This is text that will be before the counter of the loots in LootsCounter.txt
  ```
  <ScreenText>Loots:</ScreenText>
   ```
   This is for the add command to use the channel owner (broadcaster) if both are false broadcaster will always be used
   ```
  <UseChannelOwner>true</UseChannelOwner>
  ```
   This is for if you want moderators to be able to use the add / remove command
  ```
  <UseModerators>false</UseModerators>
  ```
   This is the command you will use to add or remove from the counter.
   ! will be added in the code no need for it here.
   add or remove to add or remove 1 point.
  ```
  <AddRemoveLootsCommand>LootsCounter</AddRemoveLootsCommand>
</Settings>

