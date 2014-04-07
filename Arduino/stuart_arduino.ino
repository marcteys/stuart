public bool SendMessage(string messageToArduino)
    {
        try // this carries a ton of overhead, remove if concerned about speed
        {
            // send message
            serialPort.WriteLine(messageToArduino);
        }
        #if debugSerialMessages
        catch (Exception e)
        {
            
            Debug.Log("ArduinityCommunicator::SendMessage(): " + e);
            
            return false;
        }
        #else
        catch
        {
            return false;
        }
        #endif
        return true;
    }