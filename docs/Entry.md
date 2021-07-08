WIP

### Entry

There is often a Time where given API action requires multiple steps to be fulfilled. 
One of the more prominent examples would be removong items from a prefilled container, where user would first have to check if given record exists in the container and then again look it up to remove it. That problem of extra unnecesarry lookup is easy to mitigate by a) adding more (composite)operations to the API like FindRemove, AddOrUpdate, and sometimes b) pretend the operation is happening but let the user know when it didn't, such as bool Remove(item) does. 
First solution leads to API explosion of sorts.
This design of separate operations composited outside of API might lead to many annoying problems 
Even in most basic example above - our code would have to find :While that is quite straightforward operation which is often exposed as joined operation add_overwrite 
