from enum import Enum

# Our temporary database for users
# To create user just create an instance with all hard coded values :D
class ServerClass(Enum):
    PONY = (12340, "Pony", 10, 400, 10,
            [(1, 1, 1), (2, 1, 1), (3, 3, 1), (4, 6, 16), (5, 1, 1), (6, 9, 16)])
    TIGER = (5678, "Tiger", 20, 100, 30, [])

    def __init__(self, uid, username, level, gold, armorPoints, listOfCards):
        self._uid = uid
        self._username = username
        self._level = level
        self._gold = gold
        self._armorPoints = armorPoints
        # List of triples (type, lvl, count)
        self._listOfCards = listOfCards

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

    @property
    def listOfCards(self):
        return self._listOfCards

    @classmethod
    def get_user_by_name(cls, username):
        for user in cls:
            if user.username.lower() == username.lower():
                return user
        return None

