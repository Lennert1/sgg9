from django.http import HttpResponse
import json

# default is set to ERROR message
def server_message_response(message, identifier="ERROR", extra_message="server", status=400):
    # ServerMessages are also available as models if one wants to use the messages as data for something
    return HttpResponse(server_message_json(message=message, identifier=identifier, extra_message=extra_message),
                        status=status)


# json representation
def server_message_json(message, identifier="ERROR", extra_message="server"):
    return json.dumps({
        "extraMessage": extra_message,
        "message": message,
        "identifier": identifier
    })