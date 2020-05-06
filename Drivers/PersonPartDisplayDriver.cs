using Lombiq.TrainingDemo.Models;
using Lombiq.TrainingDemo.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using System.Threading.Tasks;

namespace Lombiq.TrainingDemo.Drivers
{
    // Drivers inherited from ContentPartDisplayDrivers have a functionality similar to the one described in
    // BookDisplayDriver but these are for ContentParts. Don't forget to register this class with the service provider
    // (see: Startup.cs).
    public class PersonPartDisplayDriver : ContentPartDisplayDriver<PersonPart>
    {
        // Some notes on the various methods you can override: 
        // - Keep in mind that all of them have a sync and async version, use the one more appropriate for what you do
        //   inside them (use the async ones if you'll write any async code in the body, the sync ones otherwise).
        // - Also, some overrides will take a context object: When in doubt use the one with the context object,
        //   otherwise there is no difference apart from what you can see: The context can be passed to helper methods
        //   and is otherwise used in the background to carry some other contextual object like the current
        //   IUpdateModel. If you end up not using the context anywhere in the method's body than that means that you
        //   don't actually need it, no magic will happen behind the scenes because of it.

        // A Display method that we already know. This time it's much simpler because we don't want to create multiple
        // shapes for the PersonPart - however we could.
        public override IDisplayResult Display(PersonPart part) =>
            // Notice that there is no location given for this shape. There's another option of giving these locations
            // using Placement.json files. Since it is not possible to put comments in a .json file the explanation is
            // here but make sure you check the file while reading this. It's important to give a location somewhere
            // otherwise to shape won't be displayed. The shape file should be in the Views folder by default, however,
            // it could be outside the Views folder too (e.g. inside the Drivers folder).

            // In Placement files you can give specific locations to any shapes generated by Orchard. You can also
            // specify rules to match when the location will be applied: like only for certain fields, content types,
            // just under a given path. In our Placement file you can see that the PersonPart shape gets the first
            // position in the Content zone and the TextField shape gets the second. To make sure that not all the
            // TextFields will get the same position a "differentiator" property is given which refers to the part name
            // where the field is attached to and the field name. Make sure you also read the documentation to know
            // this feature better:
            // https://docs.orchardcore.net/en/dev/docs/reference/core/Placement/#placement-files

            // NEXT STATION: placement.json (needs to be lowercase) then come back here.
            View(nameof(PersonPart), part);

        // This is something that wasn't implemented in the BookDisplayDriver (but could've been). It will generate the
        // editor shape for the PersonPart.
        public override IDisplayResult Edit(PersonPart personPart, BuildPartEditorContext context) =>
            // Something similar to the Display method happens: you have a shape helper with a shape name possibly and
            // a factory. For editing using Initialize is the best idea. It will instantiate a view model from a type
            // given as a generic parameter. In the factory you will map the content part properties to the view model.
            // There are helper methods to generate the shape type. GetEditorShapeType() in this case will generate
            // "PersonPart_Edit".
            Initialize<PersonPartViewModel>(GetEditorShapeType(context), model =>
            {
                model.PersonPart = personPart;

                model.BirthDateUtc = personPart.BirthDateUtc;
                model.Name = personPart.Name;
                model.Handedness = personPart.Handedness;
            }).Location("Content:1");

        // NEXT STATION: Startup.cs and find the static constructor.

        // So we had an Edit (or EditAsync) that generates the editor shape now it's time to do the content
        // part-specific model binding and validation.
        public override async Task<IDisplayResult> UpdateAsync(PersonPart model, IUpdateModel updater)
        {
            var viewModel = new PersonPartViewModel();

            // Now it's where the IUpdateModel interface is really used (remember we first used it in
            // DisplayManagementController?). With this you will be able to use the Controller's model binding helpers
            // here in the driver. The prefix property will be used to distinguish between similarly named input fields
            // when building the editor form (so e.g. two content parts composing a content item can have an input
            // field called "Name"). By default Orchard Core will use the content part name but if you have multiple
            // drivers with editors for a content part you need to override it in the driver.
            await updater.TryUpdateModelAsync(viewModel, Prefix);

            // Now you can do some validation if needed. One way to do it you can simply write your own validation here
            // or you can do it in the view model class.

            // Go and check the ViewModels/PersonPartViewModel to see how to do it and then come back here.

            // Finally map the view model to the content part. By default these changes won't be persisted if there was
            // a validation error. Otherwise these will be automatically stored in the database.
            model.BirthDateUtc = viewModel.BirthDateUtc;
            model.Name = viewModel.Name;
            model.Handedness = viewModel.Handedness;

            return Edit(model);
        }
    }
}

// NEXT STATION: Controllers/PersonListController and go back to the OlderThan30 method where we left.