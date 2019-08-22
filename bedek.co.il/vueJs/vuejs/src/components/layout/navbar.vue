<template>
  <div class="navbar" style="display:block">
    <nav class="black">
      <div class="container1">
        <div class="row">
          <div class="col s3">
            <router-link :to="{name: 'buildings'}" class="brand-logo right">
              <img
                style="width:35px;"
                src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAMgAAACtCAYAAAD1YTB3AAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyNpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuNi1jMDY3IDc5LjE1Nzc0NywgMjAxNS8wMy8zMC0yMzo0MDo0MiAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wTU09Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9tbS8iIHhtbG5zOnN0UmVmPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvc1R5cGUvUmVzb3VyY2VSZWYjIiB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6NTc3NkJFREIwM0Q1MTFFOTg1NUM5REEzM0QyQTlEQzciIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6NTc3NkJFREEwM0Q1MTFFOTg1NUM5REEzM0QyQTlEQzciIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENDIChNYWNpbnRvc2gpIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6OTk2QkI4Rjk3NjE2MTFFNUE4NEU4RkIxNjQ5MTYyRDgiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6OTk2QkI4RkE3NjE2MTFFNUE4NEU4RkIxNjQ5MTYyRDgiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz7iq60ZAAAVV0lEQVR42uxdbYwc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"
              >
              <span>Bedek</span>
            </router-link>
          </div>
          <div class="col s6">
            <ul id="nav-mobile" class="nav-links">
             
              <li>
                 <router-link :to="{name: 'buildings'}">בניינים </router-link>
              </li>
               <li>
                 <router-link :to="{name: 'handyman'}">אנשי מקצוע </router-link>
              </li>
               <li>
                 <router-link :to="{name: 'serviceList'}">חוקי מכר </router-link>
              </li>               
            </ul>
          </div>
          <div class="col s3">
            <ul class="left nav-links">
              <li v-if="!user">
                <router-link :to="{name: 'Login'}">התחבר</router-link>
              </li>
               
              <li v-if="user">
                <a href="javascript:;" @click="logout">התנתק</a>
              </li>
              <li v-if="user">
                <a>{{user.email}}</a>
              </li>
            </ul>
          </div>
        </div>
      </div>
    </nav>
  </div>
</template>

<script>
export default {
  name: "Navbar",
  data() {
    return {
      user: { email: this.$store.state.loggedInUser  }
    };
  },
  created() {
    // this is fired when auth is changed (login/logout)
    /*  firebase.auth().onAuthStateChanged(user => {
      if (user) {
        this.user = user;
      } else {
        this.user = null;
      }
    }); */
  },
  methods: {
    logout() {
      localStorage.removeItem("user-token");
      this.$router.push({ name: "Login" });
    }
  }
};
</script>

<style>
.nav-links{
      padding-top: 18px;
}
.navbar .brand-logo {
  position: relative;
}
.navbar ul {
  padding-inline-start: 0;
}

.navbar li a {
  padding-right: 0;
}
</style>