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
    return  utilities.server_message_response("recieved","STATUS")

@csrf_exempt
def update_color(request):
    msg = json.dumps({
        "color": f"{ServerClass.color}"
    })
    return utilities.server_message_response(msg,"DATA")

@csrf_exempt
def update_level(request):
    msg = json.dumps({
        "lvl": f"{ServerClass.lvl}"
    })
    return utilities.server_message_response(msg,"DATA")