from enum import Enum

# Our temporary database for users
# To create user just create an instance with all hard coded values :D
class ServerClass(Enum):
    PONY = (1234, "Pony", 10, 400, 10)
    TIGER = (5678, "Tiger", 20, 100, 30)

    def __init__(self, uid, username, level, gold, armorPoints):
        self._uid = uid
        self._username = username
        self._level = level
        self._gold = gold
        self._armorPoints = armorPoints

    @property
    def uid(self):
        return self._uid

    @property
    def username(self):
        return self._username

    @property
    def level(self):
        return self._level

    @property
    def gold(self):
        return self._gold

    @property
    def armorPoints(self):
        return self._armorPoints

    @classmethod
    def get_user_by_name(cls, username):
        for user in cls:
            if user.username.lower() == username.lower():
                return user
        return None

