// This file is provided under The MIT License as part of RiptideNetworking.
// Copyright (c) Tom Weiland
// For additional information please see the included LICENSE.md file or view it on GitHub:
// https://github.com/tom-weiland/RiptideNetworking/blob/main/LICENSE.md

namespace Riptide
{
    public class MessageHandler
    {
        private Dictionary<ushort, Action<MessageReceivedEventArgs>> _subscribers;
        private Client _client;
        private Server _server;
    
        public MessageHandler(Client client)
        {
            _client = client;
            _client.MessageReceived += HandleMessage;
            _subscribers = new Dictionary<ushort, Action<MessageReceivedEventArgs>>();     
        }

        public MessageHandler(Server server)
        {
            _server = server;
            _server.MessageReceived += HandleMessage;
            _subscribers = new Dictionary<ushort, Action<MessageReceivedEventArgs>>();
        }

        public void Subscribe(ushort eventID, Action<MessageReceivedEventArgs> method)
        {
            if (_subscribers.ContainsKey(eventID))
            {
                Debug.LogError($"Event of ID: {eventID} is already subscribed to!");
                return;
            }
            _subscribers[eventID] = method;
        }

        private void HandleMessage(object sender, MessageReceivedEventArgs args)
        {
            if(!_subscribers.ContainsKey(args.MessageId))
            {
                Debug.LogError($"Message ID: {args.MessageId} does not have a handler!");
                return;
            }

            _subscribers[args.MessageId].Invoke(args);
        }
    }
}
