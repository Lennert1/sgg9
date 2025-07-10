from django.http import JsonResponse
from django.shortcuts import render
from django.views.decorators.csrf import csrf_exempt
from rest_framework.response import Response
import ServerClass
import json
import utilities

# Create your views here.
@csrf_exempt
def change_color(request):
    color_data = json.loads(request.body).get("color")
    ServerClass.color = int(color_data)
    print("Empfangene Farbe (REST):", color_data)
    return  utilities.server_message_response(f"color: {color_data}","DATA")

@csrf_exempt
def login(request):
    name = json.loads(request.body).get("name")
    uid = json.loads(request.body).get("uid")

    print("Name: ", name)
    print("UID: ", uid)
    return  utilities.server_message_response("received","STATUS")

@csrf_exempt
def register(request):
    if request.method == "POST":
        try:
            data = json.loads(request.body)
            print("Request body raw:", request.body)
            name = data.get("name")

            # The UID should be assigned by the server
            # uid = data.get("uid")

            print(f"Name: {name}")

            return utilities.server_message_response("received","STATUS", status=200)
        except Exception as e:
            return utilities.server_message_response("received","STATUS", status=400)
    else:
        return utilities.server_message_response("received","STATUS", status=405)

@csrf_exempt
def update_color(request):
    msg = json.dumps({
        "color": f"{ServerClass.color}"
    })
    return utilities.server_message_response(msg,"DATA")

